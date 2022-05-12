using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Exceptions;
using RealEstate.Application.Models;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace RealEstate.WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        public ILogger Logger { get; }

        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> Logger)
        {
            this.next = next;
            this.Logger = Logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (HttpRequestFailException error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };
                Logger.LogError(error.Message, error);
                response.StatusCode = (int)HttpStatusCode.BadGateway;
                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };
                Logger.LogError(error.Message, error);
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }
    }
}
