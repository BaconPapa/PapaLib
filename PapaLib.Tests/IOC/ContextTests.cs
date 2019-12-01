using PapaLib.IOC;
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
}