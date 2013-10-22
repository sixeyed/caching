using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Sixeyed.Caching.Caches.TableStorage;
using Sixeyed.Caching.Configuration;
using Sixeyed.Caching.Logging;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Sixeyed.Caching.Caches
{
    public class AzureTableStorageCache : CacheBase
    {
        private bool _initialised;

        public override CacheType CacheType
        {
            get { return CacheType.AzureTableStorage; }
        }

        protected override bool ItemsNeedSerializing
        {
            get { return true; }
        }

        protected override void InitialiseInternal()
        {
            if (!_initialised)
            {
                Log.Debug("AzureTableStorage.Initialise - initialising with cache name: {0}", CacheConfiguration.Current.DefaultCacheName);
                var table = GetTable();
                table.CreateIfNotExists();
                _initialised = true;
            }
        }

        protected override void SetInternal(string key, object value)
        {
            SetInternal(key, value, null);
        }

        protected override void SetInternal(string key, object value, DateTime expiresAt)
        {
            SetInternal(key, value, expiresAt);
        }

        private void SetInternal(string key, object value, DateTime? expiresAt)
        {
            var entity = new CachedEntity(key);
            entity.SerializedItem = value as string;
            entity.ExpiresAt = expiresAt;
            var table = GetTable();         
            var insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);
            table.Execute(insertOrReplaceOperation);
        }

        protected override void SetInternal(string key, object value, TimeSpan validFor)
        {
            SetInternal(key, value, DateTime.UtcNow.Add(validFor));
        }

        protected override object GetInternal(string key)
        {
            var entity = GetEntity(key);
            if (entity != null && entity.ExpiresAt.HasValue && entity.ExpiresAt < DateTime.UtcNow)
            {
                entity = null;
                var deleteKey = key;
                Task.Factory.StartNew(() => DeleteEntity(deleteKey));
            }
            return entity == null ? null : entity.SerializedItem ;
        }

        protected override void RemoveInternal(string key)
        {
            DeleteEntity(key);
        }

        protected override bool ExistsInternal(string key)
        {
            return GetInternal(key) != null;
        }

        private static void DeleteEntity(string key)
        {
            try
            {
                var entity = GetEntity(key);
                if (entity != null)
                {
                    var table = GetTable();
                    var deleteOperation = TableOperation.Delete(entity);
                    table.Execute(deleteOperation);
                }
            }
            catch(Exception ex)
            {
                Log.Warn("AzureTableStorage.DeleteEntity - failed for key: {0}, Message:", key, ex.Message);   
            }
        }

        private static CachedEntity GetEntity(string key)
        {
            var table = GetTable();
            var retrieveOperation = TableOperation.Retrieve<CachedEntity>(CachedEntity.DefaultPartitionKey, key);
            return table.Execute(retrieveOperation).Result as CachedEntity;
        }

        private static CloudStorageAccount GetStorageAccount()
        {
            var cacheName = CacheConfiguration.Current.DefaultCacheName;
            return CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings[cacheName].ConnectionString);
        }

        private static CloudTable GetTable()
        {
            var account = GetStorageAccount();
            var tableClient = account.CreateCloudTableClient();
            return tableClient.GetTableReference(typeof(CachedEntity).Name);
        }         
    }
}
