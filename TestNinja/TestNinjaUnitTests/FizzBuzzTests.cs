﻿using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class FizzBuzzTests
    {
        [Test]
        public void GetOutput_InputIsDivisibleBy3And5_ReturnFizzBuzz()
        {
            var result = FizzBuzz.GetOutput(15);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo("FizzBuzz"));
        }

        [Test]
        public void GetOutput_InputIsDivisibleBy3Only_ReturnFizz()
        {
            var result = FizzBuzz.GetOutput(3);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo("Fizz"));
        }

        [Test]
        public void GetOutput_InputIsDivisibleBy5Only_ReturnBuzz()
        {
            var result = FizzBuzz.GetOutput(5);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo("Buzz"));
        }

        [Test]
        public void GetOutput_InputIsNotDivisibleBy3Or5_ReturnTheSameNumber()
        {
            var result = FizzBuzz.GetOutput(4);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo("4"));
        }
    }
}
