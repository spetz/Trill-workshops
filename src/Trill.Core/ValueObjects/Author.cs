using System;
using Trill.Core.Exceptions;

namespace Trill.Core.ValueObjects
{
    public class Author : IEquatable<Author>
    {
        public string Name { get; }

        public Author(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidAuthorException(name);
            }
            
            Name = name;
        }
        
        public static implicit operator Author(string address) => new Author(address);

        public static implicit operator string(Author author) => author.Name;

        public bool Equals(Author other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Author) obj);
        }

        public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;
    }
}