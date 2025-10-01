using Moq;
using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceEngine.Managers;
using OrderingServiceEngine.Managers.Abstractions;

namespace TestProject
{
    public class UnitTestItem
    {
        [Fact]
        public void Test_Item_Insert()
        {
            //Arrange
            Mock<IItemDataAccess> itemDataAccess = new Mock<IItemDataAccess>();
            itemDataAccess.Setup(x => x.InsertItem(It.IsAny<OrderingServiceData.Entities.Item>())).Returns(1);

            Mock<IApplicationLogManager> applicationLogManager = new Mock<IApplicationLogManager>();
            applicationLogManager.Setup(x => x.InsertLog(It.IsAny<OrderingServiceData.Entities.APPLICATION_LOG_EVENT>(), It.IsAny<string>(), It.IsAny<long>())).Returns(1);

            ItemManager itemManager = new ItemManager(itemDataAccess.Object, applicationLogManager.Object, Mock.Of<AutoMapper.IMapper>());

            //Act
            long id = itemManager.InsertItem(1, new OrderingServiceEngine.Models.ItemModel { Name = "Test Item", Description = "This is a test item", Price = 9.99 });

            //Assert

            Assert.Equal(1, id);
        }

        [Fact]
        public void Test_Item_Insert_FAIL()
        {
            //Arrange
            Mock<IItemDataAccess> itemDataAccess = new Mock<IItemDataAccess>();
            itemDataAccess.Setup(x => x.InsertItem(It.IsAny<OrderingServiceData.Entities.Item>())).Returns(1);

            Mock<IApplicationLogManager> applicationLogManager = new Mock<IApplicationLogManager>();
            applicationLogManager.Setup(x => x.InsertLog(It.IsAny<OrderingServiceData.Entities.APPLICATION_LOG_EVENT>(), It.IsAny<string>(), It.IsAny<long>())).Returns(1);

            ItemManager itemManager = new ItemManager(itemDataAccess.Object, applicationLogManager.Object, Mock.Of<AutoMapper.IMapper>());

            //Act and Assert
            Assert.Throws<ArgumentException>(() =>
            {
                itemManager.InsertItem(1, new OrderingServiceEngine.Models.ItemModel { Name = "Test Item", Description = "This is a test item", Price = -10 });
            });

        }
    }
}