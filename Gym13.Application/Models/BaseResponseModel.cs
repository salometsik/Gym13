using Microsoft.AspNetCore.Http;

namespace Gym13.Application.Models
{
    public class BaseResponseModel
    {
        public int HttpStatusCode { get; set; } = StatusCodes.Status400BadRequest;
        public bool Success { get; set; }
        public string? UserMessage { get; set; }
        public string? DeveloperMessage { get; set; }
    }
}
