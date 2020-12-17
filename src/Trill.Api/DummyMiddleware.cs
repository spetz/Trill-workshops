using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Trill.Api
{
    public class DummyMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("Dummy middleware");
            await next(context);
        }
    }
}