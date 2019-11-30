using System;
using PapaLib.IOC;
using Xunit;

namespace PapaLib.Tests.IOC
{
    public class ContextTests
    {
        private Context context;

        public ContextTests()
        {
            context = new Context();
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
        public void RegisterSingletonForClass_GetInstance_Twice()
        {
            context.RegisterSingleton<GUIDGenerator>();
            var instance1 = context.GetInstance<GUIDGenerator>();
            var instance2 = context.GetInstance<GUIDGenerator>();
            Assert.True(instance1.guid == instance2.guid);
        }

        [Fact]
        public void RegisterPrototypeForClass_GetInstance_Twice()
        {
            context.RegisterPrototype<GUIDGenerator>();
            var instance1 = context.GetInstance<GUIDGenerator>();
            var instance2 = context.GetInstance<GUIDGenerator>();
            Assert.False(instance1.guid == instance2.guid);
        }

        [Fact]
        public void RegisterSingletonForInterface_GetInstance_Twice()
        {
            context.RegisterSingleton<IGUIDGenerator, GUIDGenerator>();
            var instance1 = context.GetInstance<IGUIDGenerator>();
            var instance2 = context.GetInstance<IGUIDGenerator>();
            Assert.True(instance1.guid == instance2.guid);
        }

        [Fact]
        public void RegisterPrototypeForInterface_GetInstance_Twice()
        {
            context.RegisterPrototype<IGUIDGenerator, GUIDGenerator>();
            var instance1 = context.GetInstance<IGUIDGenerator>();
            var instance2 = context.GetInstance<IGUIDGenerator>();
            Assert.False(instance1.guid == instance2.guid);
        }
    }
}