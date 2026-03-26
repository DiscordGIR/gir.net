using gir.net.Application.Services;
using gir.net.Domain.Entities;
using gir.net.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace gir.net.Tests.Services;

public class TagServiceTests
{
    [Fact]
    public async Task GetTagAsync_ExistingTag_ReturnsTag()
    {
        // Arrange
        var mockRepo = new Mock<ITagRepository>();
        mockRepo.Setup(repo => repo.GetTagAsync("test")).ReturnsAsync(new Tag { Name = "test", Content = "content" });
        var service = new TagService(mockRepo.Object);

        // Act
        var result = await service.GetTagAsync("test");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("content", result.Content);
    }

    [Fact]
    public async Task GetTagAsync_NonExistingTag_ReturnsNull()
    {
        // Arrange
        var mockRepo = new Mock<ITagRepository>();
        mockRepo.Setup(repo => repo.GetTagAsync("test")).ReturnsAsync((Tag?)null);
        var service = new TagService(mockRepo.Object);

        // Act
        var result = await service.GetTagAsync("test");

        // Assert
        Assert.Null(result);
    }
}
