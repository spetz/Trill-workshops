using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trill.Core.Entities;
using Trill.Core.Repositories;

namespace Trill.Infrastructure.Caching
{
    internal class InMemoryStoryRepository : IStoryRepository
    {
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