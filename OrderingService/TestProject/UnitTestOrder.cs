using Moq;
using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceEngine.Managers;
using OrderingServiceEngine.Managers.Abstractions;

namespace TestProject
{
    public class UnitTestOrder
    {
        [Fact]
        public void Test_Order_Insert()
        {
            //Arrange
            Mock<IOrderDataAccess> orderDataAccess = new Mock<IOrderDataAccess>();
            orderDataAccess.Setup(x => x.InsertOrder(It.IsAny<OrderingServiceData.Entities.Order>())).Returns(1);

            Mock<IApplicationLogManager> applicationLogManager = new Mock<IApplicationLogManager>();
            applicationLogManager.Setup(x => x.InsertLog(It.IsAny<OrderingServiceData.Entities.APPLICATION_LOG_EVENT>(), It.IsAny<string>(), It.IsAny<long>())).Returns(1);

            OrderManager itemManager = new OrderManager(orderDataAccess.Object, applicationLogManager.Object, Mock.Of<AutoMapper.IMapper>());

            //Act
            long id = itemManager.InsertOrder(new OrderingServiceEngine.Models.OrderModel()
            {
                Customer = new OrderingServiceEngine.Models.CustomerModel() { ID = 1, Name = "Antonio", Email = "semaan.antonio@lyze.com" },
                Items = new List<OrderingServiceEngine.Models.ItemModel>()
                {
                    new OrderingServiceEngine.Models.ItemModel() { ID = 1, Name = "Item 1", Description = "Description 1", Price = 10 },
                    new OrderingServiceEngine.Models.ItemModel() { ID = 2, Name = "Item 2", Description = "Description 2", Price = 20 }
                },
                OrderDate = DateTime.UtcNow,
                TotalAmount = 30
            } );

            //Assert

            Assert.Equal(1, id);
        }

        [Fact]
        public void Test_Order_Insert_FAIL()
        {
            //Arrange
            Mock<IOrderDataAccess> orderDataAccess = new Mock<IOrderDataAccess>();
            orderDataAccess.Setup(x => x.InsertOrder(It.IsAny<OrderingServiceData.Entities.Order>())).Returns(1);

            Mock<IApplicationLogManager> applicationLogManager = new Mock<IApplicationLogManager>();
            applicationLogManager.Setup(x => x.InsertLog(It.IsAny<OrderingServiceData.Entities.APPLICATION_LOG_EVENT>(), It.IsAny<string>(), It.IsAny<long>())).Returns(1);

            OrderManager itemManager = new OrderManager(orderDataAccess.Object, applicationLogManager.Object, Mock.Of<AutoMapper.IMapper>());

            //Act and Assert
            Assert.Throws<ArgumentException>(() =>
            {
                itemManager.InsertOrder(new OrderingServiceEngine.Models.OrderModel()
                {
                    Customer = new OrderingServiceEngine.Models.CustomerModel() { ID = 1, Name = "Antonio", Email = "semaan.antonio@lyze.com" },
                    Items = new List<OrderingServiceEngine.Models.ItemModel>()
                {
                    new OrderingServiceEngine.Models.ItemModel() { ID = 1, Name = "Item 1", Description = "Description 1", Price = 20 },
                    new OrderingServiceEngine.Models.ItemModel() { ID = 2, Name = "Item 2", Description = "Description 2", Price = 20 }
                },
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = 30
                });
            });

        }
    }
}