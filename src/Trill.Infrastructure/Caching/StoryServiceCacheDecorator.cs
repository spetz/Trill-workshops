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
        
        public async Task<StoryDetailsDto> GetAsync(Guid id)
        {
            var key = GetCacheKey(id);
            var dto = _cache.Get<StoryDetailsDto>(key);
            if (dto is {})
            {
                return dto;
            }

            dto = await _storyService.GetAsync(id);
            if (dto is null)
            {
                return null;
            }
        
            _cache.Set(key, dto);

            return dto;
        }

        public Task<IEnumerable<StoryDto>> BrowseAsync(string author = null) => _storyService.BrowseAsync(author);

        public Task AddAsync(SendStory command) => _storyService.AddAsync(command);
        
        private static string GetCacheKey(Guid id) => $"stories:{id:N}";
    }
}