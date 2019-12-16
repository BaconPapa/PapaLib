using System;
using PapaLib.IOC.Attributes;
using Xunit;

namespace PapaLib.Tests.IOC
{
    public class ContextRegisterByCodeTests : ContextTests
    {
        private interface IGuidGenerator
        {
            Guid Guid { get; }
        }

        private class RandomGuidGenerator : IGuidGenerator
        {
            public Guid Guid { get; } = Guid.NewGuid();
        }

        private interface ICalculator
        {
            float Calculate(float a, float b);
        }

        private class AddCalculator : ICalculator
        {
            public float Calculate(float a, float b) => a + b;
        }

        private class Calculator
        {
            private ICalculator AddCalculator { get; }
            public Calculator(ICalculator addAddCalculator)
            {
                AddCalculator = addAddCalculator;
            }

            public float Add(float a, float b) => AddCalculator.Calculate(a, b);
        }
        
        private class ContextConfig
        {
            [Bean]
            public static IGuidGenerator CreateGuidGenerator()
            {
                return new RandomGuidGenerator();
            }

            [Bean]
            public static Calculator CreateCalculator(ICalculator addCalculator)
            {
                return new Calculator(addCalculator);
            }

            [Bean]
            public static ICalculator CreateICalculator()
            {
                return new AddCalculator();
            }
        }
        [Fact]
        public void RegisterByCodeTests()
        {
            Context.RegisterContext<ContextConfig>();
            var generator = Context.GetInstance<IGuidGenerator>();
            var generator2 = Context.GetInstance<IGuidGenerator>();
            Assert.True(generator.Guid == generator2.Guid);
            var calculator = Context.GetInstance<Calculator>();
            Assert.Equal(2f, calculator.Add(1f, 1f));
        }
    }
}