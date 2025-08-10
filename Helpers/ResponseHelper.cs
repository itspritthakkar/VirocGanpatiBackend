using Microsoft.AspNetCore.Mvc;

namespace VirocGanpati.Helpers
{
    public class ResponseHelper
    {
        public static IActionResult UnauthorizedMessage(string? message)
        {
            return new UnauthorizedObjectResult(new { Message = message ?? "You do not access to this resource" });
        }

        public static ActionResult<T> UnauthorizedMessage<T>(string? message)
        {
            return new UnauthorizedObjectResult(new { Message = message ?? "You do not access to this resource" });
        }

        public static IActionResult NotFoundMessage(string? message)
        {
            return new NotFoundObjectResult(new { Message = message ?? "Resource not found" });
        }

        public static ActionResult<T> NotFoundMessage<T>(string? message)
        {
            return new NotFoundObjectResult(new { Message = message ?? "Resource not found" });
        }

        public static IActionResult BadRequestMessage(string? message)
        {
            return new BadRequestObjectResult(new { Message = message ?? "There was an error in the request" });
        }

        public static ActionResult<T> BadRequestMessage<T>(string? message)
        {
            return new BadRequestObjectResult(new { Message = message ?? "There was an error in the request" });
        }

        public static IActionResult OkMessage(string? message)
        {
            return new OkObjectResult(new { Message = message ?? "Operation successful" });
        }

        public static IActionResult InternalServerErrorMessage(string? message)
        {
            return new ObjectResult(new { Message = message ?? "An unexpected error occurred" })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public static ActionResult<T> InternalServerErrorMessage<T>(string? message)
        {
            return new ObjectResult(new { Message = message ?? "An unexpected error occurred" })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public static IActionResult ConflictMessage(string? message)
        {
            return new ObjectResult(new { Message = message ?? "An unexpected error occurred" })
            {
                StatusCode = StatusCodes.Status409Conflict
            };
        }

        public static ActionResult<T> ConflictMessage<T>(string? message)
        {
            return new ObjectResult(new { Message = message ?? "An unexpected error occurred" })
            {
                StatusCode = StatusCodes.Status409Conflict
            };
        }
    }
}
