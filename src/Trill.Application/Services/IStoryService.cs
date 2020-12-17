using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trill.Application.Commands;
using Trill.Application.DTO;

namespace Trill.Application.Services
{
    public interface IStoryService
    {
        Task<StoryDetailsDto> GetAsync(Guid id);
        Task<IEnumerable<StoryDto>> BrowseAsync(string author = null);
        Task AddAsync(SendStory command);
    }
}