using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Trill.Application.Commands;
using Trill.Application.Services;
using Trill.Core.Entities;
using Trill.Core.Exceptions;
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
        
        [Fact]
        public async Task add_should_succeed_given_valid_data()
        {
            // Arrange
            var command = new SendStory(Guid.NewGuid(), "test", "Lorem ipsum", "user1", new[] {"tag1", "tag2"});
            
            // Act
            await _storyService.AddAsync(command);
            
            // Assert
            await _storyRepository.Received(1).AddAsync(Arg.Is<Story>(x =>
                x.Id == command.Id &&
                x.Title == command.Title &&
                x.Text == command.Text &&
                x.Author == command.Author));
        }

        [Fact]
        public async Task add_should_fail_given_missing_title()
        {
            // Arrange
            var command = new SendStory(Guid.NewGuid(), default, "Lorem ipsum", "user1", new[] {"tag1", "tag2"});
            
            // Act
            var exception = await Record.ExceptionAsync(() => _storyService.AddAsync(command));
            
            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<MissingTitleException>();
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