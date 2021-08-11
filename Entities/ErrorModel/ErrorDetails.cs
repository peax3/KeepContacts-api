using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.ErrorModel
{
   public class ErrorDetails
   {
      public int StatusCode { get; set; }
      public string Message { get; set; }
      public string Details { get; set; }

      public ErrorDetails(string message, int statusCode = 500, string details = null)
      {
         this.Message = message;
         this.StatusCode = statusCode;
         this.Details = details;
      }

      public override string ToString()
      {
         var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
         return JsonSerializer.Serialize(this, options);
      }
   }
}