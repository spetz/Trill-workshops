using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trill.Infrastructure;

namespace Trill.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class HomeController : ControllerBase
    {
        private static readonly List<string> Messages = new List<string>
        {
            "hello", "hi", "hey"
        };
        private readonly IOptions<ApiOptions> _options;

        public HomeController(IOptions<ApiOptions> options)
        {
            _options = options;
        }

        [HttpGet]
        public ActionResult<string> Get() => Ok(_options.Value.Name);

        [HttpGet("messages")]
        public ActionResult<List<string>> Browse([FromQuery] Query query) => Ok(Messages);

        [HttpPost("messages")]
        public ActionResult Post(Message message)
        {
            Messages.Add(message.Name);
            return NoContent();
        }

        public class Message
        {
            [Required]
            [StringLength(10, MinimumLength = 3)]
            public string Name { get; set; }
        }

        public class Query
        {
            public string Message { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}