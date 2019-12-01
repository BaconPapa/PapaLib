using System;
using PapaLib.IOC;
using PapaLib.IOC.Attributes;
using Xunit;

namespace PapaLib.Tests.IOC
{
    public class ContextTests
    {
        protected readonly Context Context;

        public ContextTests()
        {
            Context = new Context();
        }
    }

    public class ContextRegisterAndInstanceTests : ContextTests
    {
        private interface IGuidGenerator
        {
            Guid Guid {get;}
        }

        private class GuidGenerator : IGuidGenerator
        {
            public Guid Guid { get; }
            public GuidGenerator()
            {
                Guid = Guid.NewGuid();
            }
        }

        [Fact]
        public void RegisterSingletonForClass_GetInstance_Twice()
        {
            Context.RegisterSingleton<GuidGenerator>();
            var instance1 = Context.GetInstance<GuidGenerator>();
            var instance2 = Context.GetInstance<GuidGenerator>();
            Assert.True(instance1.Guid == instance2.Guid);
        }

        [Fact]
        public void RegisterPrototypeForClass_GetInstance_Twice()
        {
            Context.RegisterPrototype<GuidGenerator>();
            var instance1 = Context.GetInstance<GuidGenerator>();
            var instance2 = Context.GetInstance<GuidGenerator>();
            Assert.False(instance1.Guid == instance2.Guid);
        }

        [Fact]
        public void RegisterSingletonForInterface_GetInstance_Twice()
        {
            Context.RegisterSingleton<IGuidGenerator, GuidGenerator>();
            var instance1 = Context.GetInstance<IGuidGenerator>();
            var instance2 = Context.GetInstance<IGuidGenerator>();
            Assert.True(instance1.Guid == instance2.Guid);
        }

        [Fact]
        public void RegisterPrototypeForInterface_GetInstance_Twice()
        {
            Context.RegisterPrototype<IGuidGenerator, GuidGenerator>();
            var instance1 = Context.GetInstance<IGuidGenerator>();
            var instance2 = Context.GetInstance<IGuidGenerator>();
            Assert.False(instance1.Guid == instance2.Guid);
        }
    }

    public class ContextSingletonLoadModTests : ContextTests
    {
        public ContextSingletonLoadModTests()
        {
            LoadingChecker.InstantiateCount = 0;
        }
        private class LoadingChecker
        {
            public static int InstantiateCount;
            public LoadingChecker()
            {
                InstantiateCount ++;
            }
        }
        [Fact]
        public void RegisterSingletonForClass_InTimeMod()
        {
            Context.RegisterSingleton<LoadingChecker>();
            Assert.Equal(1, LoadingChecker.InstantiateCount);
        }
        [Fact]
        public void RegisterSingletonForClass_LazyMod()
        {
            Context.RegisterSingleton<LoadingChecker>(SingletonLoadMode.Lazy);
            Assert.Equal(0, LoadingChecker.InstantiateCount);
            Context.GetInstance<LoadingChecker>();
            Assert.Equal(1, LoadingChecker.InstantiateCount);
        }
    }

    public class ContextRegisterWithDependencyTests : ContextTests
    {
        private class Adder
        {
            public int Calculate(int a, int b) => a + b;
        }

        private class ConstructorCalculator
        {
            private readonly Adder _adder;
            public ConstructorCalculator(Adder adder)
            {
                _adder = adder;
            }

            public int Add(int a, int b) => _adder.Calculate(a, b);
        }

        [Fact]
        public void Singleton_InTime_Single_Dependency_From_Constructor()
        {
            Context.RegisterSingleton<Adder>();
            Context.RegisterSingleton<ConstructorCalculator>();
            var calculator = Context.GetInstance<ConstructorCalculator>();
            Assert.Equal(2, calculator.Add(1, 1));
        }

        private class ReferenceFieldCalculator
        {
            [Reference]
            private Adder _adder;
            public int Add(int a, int b) => _adder.Calculate(a, b);
        }

        [Fact]
        public void Singleton_InTime_Single_Dependency_Field_Reference()
        {
            Context.RegisterSingleton<Adder>();
            Context.RegisterSingleton<ReferenceFieldCalculator>();
            var calculator = Context.GetInstance<ReferenceFieldCalculator>();
            Assert.Equal(2, calculator.Add(1, 1));
        }
    }
}