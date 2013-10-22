using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Caching.Tests.Stubs
{
    public interface IContainedStub 
    {
        int GetInt();
    }

    public interface IOtherStub
    {
    }

    public class ContainedStub : IContainedStub
    {
        private static Random _Random = new Random();

        private int _int;

        public ContainedStub()
        {
            _int = _Random.Next();
        }

        public int GetInt()
        {
            return _int;
        }
    }

    public class OtherContainedStub : IContainedStub
    {
        public int GetInt()
        {
            throw new NotImplementedException();
        }
    }
}
