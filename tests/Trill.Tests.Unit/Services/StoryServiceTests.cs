using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Trill.Application.Services;
using Trill.Core.Entities;
using Trill.Core.Repositories;
using Xunit;

namespace Trill.Tests.Unit.Services
{
    public class StoryServiceTests
    {
        [Fact]
        public async Task get_story_should_return_dto_given_valid_id()
        {
            // Arrange
            var story = new Story(Guid.NewGuid(), "Test", "Lorem ipsum",
                "Author", new[] {"tag1"}, DateTime.UtcNow);
            _storyRepository.GetAsync(story.Id).Returns(story);

            // Act
            var dto = await _storyService.GetAsync(story.Id);
            
            // Assert
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(story.Id);
            dto.Title.ShouldBe(story.Title);
            await _storyRepository.Received(1).GetAsync(story.Id);
        }

        private readonly IStoryRepository _storyRepository;
        private readonly ILogger<StoryService> _logger;
        private readonly IStoryService _storyService;

        public StoryServiceTests()
        {
            _storyRepository = Substitute.For<IStoryRepository>();
            _logger = Substitute.For<ILogger<StoryService>>();
            _storyService = new StoryService(_storyRepository, _logger);
        }
    }
}