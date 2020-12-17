using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trill.Application.Commands;
using Trill.Application.DTO;

namespace Trill.Application.Services
{
    public class StoryService : IStoryService
    {
        // This is not thread-safe (use Concurrent collection)
        private static readonly List<StoryDetailsDto> Stories = new List<StoryDetailsDto>();
        
        public Task<StoryDetailsDto> GetAsync(Guid id) => Task.FromResult(Stories.SingleOrDefault(p => p.Id == id));
    
        public Task<IEnumerable<StoryDto>> BrowseAsync(string author = null)
            => Task.FromResult(Stories.Where(x => author is null || x.Author == author)
                .Select(x => new StoryDto
                {
                    Id = x.Id,
                    Author = x.Author,
                    Title = x.Title,
                    Tags = x.Tags ?? Enumerable.Empty<string>(),
                    CreatedAt = x.CreatedAt
                }));
            
        public async Task AddAsync(SendStory command)
        {
            var story = await GetAsync(command.Id);
            if (story is {})
            {
                throw new Exception($"Story with ID: '{command.Id}' already exists.");
            }
        
            Stories.Add(new StoryDetailsDto
            {
                Id = command.Id,
                Author = command.Author,
                Title = command.Title,
                Text = command.Text,
                CreatedAt = DateTime.UtcNow,
                Tags = command.Tags
            });
        }
    }
}