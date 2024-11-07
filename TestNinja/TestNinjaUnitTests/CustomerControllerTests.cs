using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinjaUnitTests
{
    [TestFixture]
    public class CustomerControllerTests
    {
        /// <summary>
        /// Testing the Return Type of Methods.
        /// </summary>
        [Test]
        public void GetCustomer_IdIsZero_ReturnNotFound()
        {
            var controller = new CustomerController();

            var result = controller.GetCustomer(0);

            Assert.That(result, Is.TypeOf<NotFound>());
        }
    }
}
