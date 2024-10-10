using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class ErrorLoggerTests
    {
        private ErrorLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new ErrorLogger();
        }

        /// <summary>
        /// Testing Void Methods.
        /// </summary>
        [Test]
        public void Log_WhenCalled_SetTheLastErrorProperty()
        {
            _logger.Log("Teste");

            Assert.That(_logger.LastError, Is.EqualTo("Teste"));
        }

        /// <summary>
        /// Testing Methods that Throw Exceptions.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Log_InvalidError_ThrowArgumentNullException(string error)
        {
            Assert.That(() => _logger.Log(error), Throws.ArgumentNullException);
        }

        /// <summary>
        /// Testing Methods that Raise an Event.
        /// </summary>
        [Test]
        public void Log_ValidError_RaiseErrorLoggedEvent()
        {
            var id = Guid.Empty;
            _logger.ErrorLogged += (sender, args) => { id = args; };

            _logger.Log("Event");

            Assert.That(id, Is.Not.EqualTo(Guid.Empty));
        }
    }
}
