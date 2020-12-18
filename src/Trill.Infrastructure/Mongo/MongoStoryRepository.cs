using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Core.Entities;
using Trill.Core.Repositories;

namespace Trill.Infrastructure.Mongo
{
    internal class MongoStoryRepository : IStoryRepository
    {
        private readonly IMongoCollection<StoryDocument> _collection;

        public MongoStoryRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<StoryDocument>("stories");
        }

        public async Task<Story> GetAsync(Guid id)
        {
            var story = await _collection
                .AsQueryable()
                .SingleOrDefaultAsync(d => d.Id == id);

            return story?.ToEntity();
        }

        public async Task<IEnumerable<Story>> BrowseAsync(string author = null)
        {
            var stories = await _collection
                .AsQueryable()
                .Where(x => author == null || x.Author == author)
                .ToListAsync();

            return stories.Select(x => x.ToEntity());
        }

        public Task AddAsync(Story story)
            => _collection.InsertOneAsync(new StoryDocument(story));
    }
}