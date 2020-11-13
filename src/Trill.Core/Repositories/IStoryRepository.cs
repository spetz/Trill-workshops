using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trill.Core.Entities;

namespace Trill.Core.Repositories
{
    public interface IStoryRepository
    {
        Task<Story> GetAsync(Guid id);
        Task<IEnumerable<Story>> BrowseAsync(string author = null);
        Task AddAsync(Story story);
    }
}