using System;
using PapaLib.IOC;
using Xunit;

namespace PapaLib.Tests.IOC
{
    public class CodeContextTests
    {
        private CodeContext codeContext;

        public CodeContextTests()
        {
            codeContext = new CodeContext();
        }

        private interface IGUIDGenerator
        {
            Guid guid {get;}
        }

        private class GUIDGenerator : IGUIDGenerator
        {
            public Guid guid { get; }
            public GUIDGenerator()
            {
                guid = Guid.NewGuid();
            }
        }

        [Fact]
        public void RegisterSingleton_GetInstance_Twice()
        {
            codeContext.RegisterSingleton<GUIDGenerator>();
            var instance1 = codeContext.GetInstance<GUIDGenerator>();
            var instance2 = codeContext.GetInstance<GUIDGenerator>();
            Assert.True(instance1 == instance2);
        }

        [Fact]
        public void RegisterPrototype_GetInstance_Twice()
        {
            codeContext.RegisterPrototype<GUIDGenerator>();
            var instance1 = codeContext.GetInstance<GUIDGenerator>();
            var instance2 = codeContext.GetInstance<GUIDGenerator>();
            Assert.False(instance1 == instance2);
        }
    }
}