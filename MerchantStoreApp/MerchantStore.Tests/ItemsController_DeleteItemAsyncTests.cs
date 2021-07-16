using System;
using System.Threading.Tasks;
using FluentAssertions;
using MerchantStoreApi.Controllers;
using MerchantStoreApi.Entities;
using MerchantStoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MerchantStore.Tests
{
    public class ItemsController_DeleteItemAsyncTests
    {
        private readonly Mock<IItemsRepository> _repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> _loggerStub = new();
        private readonly Random _random = new();

        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            Item existingItem = CreateRandomItem();

            _repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);
         

            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);
            // Act
            var result = await controller.DeleteItem(existingItem.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();

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
