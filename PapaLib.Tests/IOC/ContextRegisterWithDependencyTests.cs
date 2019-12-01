using PapaLib.IOC.Attributes;
using Xunit;

namespace PapaLib.Tests.IOC
{
    public class ContextRegisterWithDependencyTests : ContextTests
    {
        private interface ICalculator
        {
            int Calculate(int a, int b);
        }
        private class Adder : ICalculator
        {
            public int Calculate(int a, int b) => a + b;
        }

        private class Multiplier : ICalculator
        {
            public int Calculate(int a, int b) => a * b;
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
        
        private class ReferencePropertyCalculator
        {
            [Reference]
            private Adder Adder { get; set; }
            public int Add(int a, int b) => Adder.Calculate(a, b);
        }

        [Fact]
        public void Singleton_InTime_Single_Dependency_Property_Reference()
        {
            Context.RegisterSingleton<Adder>();
            Context.RegisterSingleton<ReferencePropertyCalculator>();
            var calculator = Context.GetInstance<ReferencePropertyCalculator>();
            Assert.Equal(2, calculator.Add(1, 1));
        }

        private class MultiCalculator
        {
            private readonly ICalculator _adder;
            private readonly ICalculator _multiplier;

            public MultiCalculator(
                [Reference("Adder")]ICalculator adder, 
                [Reference("Multiplier")]ICalculator multiplier
            )
            {
                _adder = adder;
                _multiplier = multiplier;
            }

            public int Add(int a, int b) => _adder.Calculate(a, b);

            public int Multiple(int a, int b) => _multiplier.Calculate(a, b);
        }

        [Fact]
        public void Singleton_InTime_Double_Dependency_Property_Reference()
        {
            Context.RegisterSingleton<Adder>();
            Context.RegisterSingleton<Multiplier>();
            Context.RegisterSingleton<MultiCalculator>();
            var calculator = Context.GetInstance<MultiCalculator>();
            Assert.Equal(2, calculator.Add(1, 1));
            Assert.Equal(4, calculator.Multiple(2, 2));
        }
    }
}