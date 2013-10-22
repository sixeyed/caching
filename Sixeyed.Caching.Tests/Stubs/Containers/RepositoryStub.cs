using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Caching.Tests.Stubs
{
    public interface IRepository {}

    public interface IUserRepository : IRepository { }

    public interface IAccountRepository : IRepository { }

    public class UserRepositoryStub : IUserRepository { }

    public class AccountRepositoryStub : IAccountRepository { }
}
