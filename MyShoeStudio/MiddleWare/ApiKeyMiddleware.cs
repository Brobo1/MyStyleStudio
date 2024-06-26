﻿namespace MyShoeStudio.MiddleWear
{
    public class ApiKeyMiddleware
    {
        private const string ApiKeyHeaderName = "_X_API_KEY_";
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var receivedApiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key was not provided");
                return;
            }

            var apiKey = Environment.GetEnvironmentVariable("_X_API_KEY_");
            if (!apiKey.Equals(receivedApiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized client");
                return;
            }

            await _next(context);
        }
    }

}
