using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class OrderServiceTests
    {
        [Test]
        public void PlaceOrder_WhenCalled_StoreTheOrder()
        {
            var store = new Mock<IStorage>();
            var service = new OrderService(store.Object);

            var order = new Order();
            service.PlaceOrder(order);

            store.Verify(s => s.Store(order));
        }
    }
}
