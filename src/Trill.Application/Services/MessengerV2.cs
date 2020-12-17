using System;

namespace Trill.Application.Services
{
    public class MessengerV2 : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();

        public string GetMessage() => $"Hello v2: {_id}";
    }
}