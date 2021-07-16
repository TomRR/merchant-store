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
    public class ItemsController_CreateItemAsyncTests
    {
        private readonly Mock<IItemsRepository> _repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> _loggerStub = new();
        private readonly Random _random = new();

        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange
            var itemToCreate = new CreateItemDto(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                _random.Next(1000));

            var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);
            // Act
            var result = await controller.CreateItemAsync(itemToCreate);

            // Assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
                );
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }
    }
}
