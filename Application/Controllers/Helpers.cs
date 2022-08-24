using Application.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public static class ControllerHelpers
    {
        public static ObjectResult ReturnError(Exception e, int statusCode, ILogger _logger, Func<int, ErrorResponseDTO, ObjectResult> StatusCode)
        {
            _logger.LogError(e.ToString(), e.StackTrace);
            ErrorResponseDTO messageDTO = new ErrorResponseDTO { Message = e.Message, CodeError = statusCode };
            return StatusCode(statusCode, messageDTO);
        }

        public static ObjectResult ReturnError(int statusCode, object? value, ILogger _logger, Func<int,ErrorResponseDTO, ObjectResult> StatusCode)
        {
            _logger.LogError(value.ToString());
            ErrorResponseDTO messageDTO = new ErrorResponseDTO { Message = value.ToString(), CodeError = statusCode };
            return StatusCode(statusCode, messageDTO);
        }
    }
}
