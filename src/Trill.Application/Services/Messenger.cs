using System;

namespace Trill.Application.Services
{
    public class Messenger : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();

        public string GetMessage() => $"Hello: {_id}";
    }
}