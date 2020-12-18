using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Trill.Application.Commands;
using Trill.Application.DTO;
using Trill.Application.Services;

namespace Trill.Infrastructure.Caching
{
    internal class StoryServiceCacheDecorator : IStoryService
    {
        private readonly IStoryService _storyService;
        private readonly IMemoryCache _cache;

        public StoryServiceCacheDecorator(IStoryService storyService, IMemoryCache cache)
        {
            _storyService = storyService;
            _cache = cache;
        }
        
        public Task<StoryDetailsDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StoryDto>> BrowseAsync(string author = null)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(SendStory command)
        {
            throw new NotImplementedException();
        }
    }
}