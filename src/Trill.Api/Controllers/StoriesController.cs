using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Trill.Application.Commands;
using Trill.Application.DTO;
using Trill.Application.Services;

namespace Trill.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoriesController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoryDto>>> Get(string author)
            => Ok(await _storyService.BrowseAsync(author));
    
        [HttpGet("{storyId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StoryDetailsDto>> Get(Guid storyId)
        {
            var story = await _storyService.GetAsync(storyId);
            if (story is null)
            {
                return NotFound();
            }

            return Ok(story);
        }
    
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post(SendStory command)
        {
            await _storyService.AddAsync(command);

            return CreatedAtAction(nameof(Get), new {storyId = command.Id}, null);
        }
    }
}