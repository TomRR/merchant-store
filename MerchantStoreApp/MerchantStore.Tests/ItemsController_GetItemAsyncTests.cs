using System;
using System.Threading.Tasks;
using FluentAssertions;
using MerchantStoreApi;
using MerchantStoreApi.Controllers;
using MerchantStoreApi.Entities;
using MerchantStoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MerchantStore.Tests
{
    public class ItemsController_GetItemAsyncTests 
    {
        private readonly Mock<IItemsRepository> _repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> _loggerStub = new();
        private readonly Random _random = new();

        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound_WithoutFluentAssertions()
        {
            // Arrange
            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem_WithoutFluentAssertions()
        {
            // Arrange
            var expectedItem = CreateRandomItem();

            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            Assert.IsType<ItemDto>(result.Value);
            var dto = (result as ActionResult<ItemDto>).Value;
            Assert.Equal(expectedItem.Id, dto.Id);
            Assert.Equal(expectedItem.Name, dto.Name);
            Assert.Equal(expectedItem.Price, dto.Price);
            Assert.Equal(expectedItem.CreatedDate, dto.CreatedDate);
        }


        // With Package FluentAssertions 44min TimeStamp
        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound_WithFluentAssertions()
        {
            // Arrange
            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem_WithFluentAssertions()
        {
            // Arrange
            var expectedItem = CreateRandomItem();

            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);
            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(
                    expectedItem,
                    options => options.ComparingByMembers<Item>() // nur noetig bei record types - kann entfernt werden
                    );
        }



        private Item CreateRandomItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = _random.Next(100000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
