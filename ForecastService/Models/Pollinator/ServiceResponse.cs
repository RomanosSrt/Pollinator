using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Models.Pollinator
{
    public class ServiceResponse<T>
    {
        public T? Response { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

        public ServiceResponse(T response)
        {
            Response = response;
        }

        public ServiceResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
