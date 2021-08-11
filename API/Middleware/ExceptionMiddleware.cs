using Entities.ErrorModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Middleware
{
   public class ExceptionMiddleware
   {
      private readonly RequestDelegate _next;
      private readonly IHostEnvironment _env;
      private readonly ILogger<ExceptionMiddleware> _logger;

      public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionMiddleware> logger)
      {
         this._next = next;
         this._env = env;
         this._logger = logger;
      }

      public async Task InvokeAsync(HttpContext httpContext)
      {
         try
         {
            await _next(httpContext);
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, ex.Message);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ErrorDetails response;

            if (_env.IsDevelopment())
               response = new ErrorDetails(ex.Message, httpContext.Response.StatusCode, ex.StackTrace);
            else
            {
               response = new ErrorDetails(ex.Message, httpContext.Response.StatusCode);
            }

            await httpContext.Response.WriteAsync(response.ToString());
         }
      }
   }
}