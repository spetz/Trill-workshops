using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
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
        
        public Task<Story> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Story>> BrowseAsync(string author = null)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Story story)
        {
            throw new NotImplementedException();
        }
    }
}