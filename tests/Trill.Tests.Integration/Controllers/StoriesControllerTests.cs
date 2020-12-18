using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using Trill.Api;
using Trill.Application.DTO;
using Trill.Core.Entities;
using Trill.Core.Repositories;
using Xunit;

namespace Trill.Tests.Integration.Controllers
{
    // [Collection("stories")] // Run tests sequentially
    public class StoriesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task get_story_should_return_not_found_given_invalid_id()
        {
            var storyId = Guid.NewGuid();
            var response = await _client.GetAsync($"api/stories/{storyId}");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task get_story_should_return_dto_given_valid_id()
        {
            // Arrange
            var story = new Story(Guid.NewGuid(), "Test", "Lorem ipsum",
                "Author", new[] {"tag1"}, DateTime.UtcNow);
            await _storyRepository.AddAsync(story);
            
            // Act
            var response = await _client.GetAsync($"api/stories/{story.Id}");
            
            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var dto = JsonConvert.DeserializeObject<StoryDetailsDto>(await response.Content.ReadAsStringAsync());
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(story.Id);
        }

        private readonly IStoryRepository _storyRepository;
        private readonly HttpClient _client;

        public StoriesControllerTests(WebApplicationFactory<Startup> factory)
        {
            _storyRepository = new TestStoryRepository();
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton(_storyRepository);
                    });
                    builder.UseEnvironment("test");
                })
                .CreateClient();
        }
    }
    
    internal class TestStoryRepository : IStoryRepository
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