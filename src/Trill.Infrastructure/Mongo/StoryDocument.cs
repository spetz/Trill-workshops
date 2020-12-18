using System;
using System.Collections.Generic;
using Trill.Core.Entities;

namespace Trill.Infrastructure.Mongo
{
    internal class StoryDocument
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public DateTime CreatedAt { get; set; }

        public StoryDocument()
        {
        }

        public StoryDocument(Story story)
        {
            Id = story.Id;
            Title = story.Title;
            Text = story.Text;
            Author = story.Author;
            Tags = story.Tags;
            CreatedAt = story.CreatedAt;
        }

        public Story ToEntity() => new Story(Id, Title, Text, Author, Tags, CreatedAt);
    }
}