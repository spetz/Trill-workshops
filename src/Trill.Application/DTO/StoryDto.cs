using System;
using System.Collections.Generic;

namespace Trill.Application.DTO
{
    public class StoryDto
    {
        public Guid Id { get; set; }
        public string Title { get;set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string> Tags { get;set; }
    }
}