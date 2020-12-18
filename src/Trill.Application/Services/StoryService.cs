using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Application.Commands;
using Trill.Application.DTO;
using Trill.Core.Entities;
using Trill.Core.Exceptions;
using Trill.Core.Repositories;
using Trill.Core.ValueObjects;

namespace Trill.Application.Services
{
    internal class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly ILogger<StoryService> _logger;

        public StoryService(IStoryRepository storyRepository, ILogger<StoryService> logger)
        {
            _storyRepository = storyRepository;
            _logger = logger;
        }

        public async Task<StoryDetailsDto> GetAsync(Guid id)
        {
            var story = await _storyRepository.GetAsync(id);

            return story is null
                ? null
                : new StoryDetailsDto
                {
                    Id = story.Id,
                    Author = story.Author,
                    Text = story.Text,
                    Title = story.Title,
                    Tags = story.Tags ?? Enumerable.Empty<string>(),
                    CreatedAt = story.CreatedAt
                };
        }

        public async Task<IEnumerable<StoryDto>> BrowseAsync(string author = null)
        {
            var stories = await _storyRepository.BrowseAsync(author);

            return stories.Select(x => new StoryDto
            {
                Id = x.Id,
                Author = x.Author,
                Title = x.Title,
                Tags = x.Tags ?? Enumerable.Empty<string>(),
                CreatedAt = x.CreatedAt
            });
        }

        public async Task AddAsync(SendStory command)
        {
            var author = new Author(command.Author);
            var story = new Story(command.Id, command.Title, command.Text, author, command.Tags, DateTime.UtcNow);
            await _storyRepository.AddAsync(story);
            _logger.LogInformation($"Added a story with ID: '{story.Id}'.");
        }
    }
}