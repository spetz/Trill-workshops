using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trill.Core.Entities;
using Trill.Core.Repositories;

namespace Trill.Infrastructure.Caching
{
    internal class InMemoryStoryRepository : IStoryRepository
    {
        // This is not thread-safe (use Concurrent collection)
        private static readonly List<Story> Stories = new List<Story>();
        
        public Task<Story> GetAsync(Guid id) => Task.FromResult(Stories.SingleOrDefault(p => p.Id == id));

        public Task<IEnumerable<Story>> BrowseAsync(string author = null)
            => Task.FromResult(Stories.Where(x => author is null || x.Author == author));

        public async Task AddAsync(Story story)
        {
            if (await GetAsync(story.Id) is {})
            {
                throw new Exception($"Story with ID: '{story.Id}' already exists.");
            }

            Stories.Add(story);
        }
    }
}