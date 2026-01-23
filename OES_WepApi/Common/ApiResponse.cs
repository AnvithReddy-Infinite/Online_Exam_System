using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }      // true if operation succeeded
        public string Message { get; set; }    // success/error message
        public T Data { get; set; }            // optional payload

        public ApiResponse() { }

        public ApiResponse(bool success, string message, T data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}