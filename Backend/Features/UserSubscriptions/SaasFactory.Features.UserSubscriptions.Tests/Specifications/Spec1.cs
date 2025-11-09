using NUnit.Framework;
using Microsoft.Playwright.NUnit;
using TestStack.BDDfy;

namespace Demo.Tests
{
    [TestFixture]
    public class Calculator_Addition_Spec: PageTest
    {
        private Calculator _calculator;
        private int _result;

        [Test]
        public void RunStory()
        {
            this.BDDfy(); // Runs the Given/When/Then steps
        }

        void Given_a_calculator()
        {
            _calculator = new Calculator();
        }

        void When_I_add_2_and_3()
        {
            _result = _calculator.Add(2, 3);
        }

        void Then_the_result_should_be_5()
        {
            Assert.That(_result, Is.EqualTo(5));
        }
    }

    public class Calculator
    {
        public int Add(int a, int b) => a + b;
    }
}