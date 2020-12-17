using System;
using System.Collections.Generic;

namespace Trill.Application.Commands
{
    public class SendStory
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Text { get; }
        public string Author { get; }
        public IEnumerable<string> Tags { get; }

        public SendStory(Guid id, string title, string text, string author, IEnumerable<string> tags)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Title = title;
            Text = text;
            Author = author;
            Tags = tags;
        }
    }
}