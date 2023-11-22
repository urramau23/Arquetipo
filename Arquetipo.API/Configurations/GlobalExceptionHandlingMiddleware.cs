using Arquetipo.Core.Entities.Exceptions;
using System.Net;
using System.Text.Json;

namespace Arquetipo.API.Filters
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _log;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next,
                                                ILogger<GlobalExceptionHandlingMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke (HttpContext context)
        {
            try
            {
                await _next(context);
                
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var exceptionType = ex.GetType();
            int codeResult = 500;
            GlobalException globalException = new GlobalException();

            if(exceptionType == typeof(BusinessException))
            {
                // Error de negocio
                codeResult = (int)HttpStatusCode.BadRequest;
                var excep = (BusinessException)ex;
                globalException.Codigo = excep.Code;
                globalException.Message = excep.Message;
            }
            else if(exceptionType == typeof(NotFoundException))
            {
                // Recurso no encontrado
                codeResult = (int)HttpStatusCode.NotFound;
                var excep = (NotFoundException)ex;
                globalException.Codigo = excep.Code;
                globalException.Message = excep.Message;
            }
            else if (exceptionType == typeof(InternalServerErrorException))
            {
                // Error en algun componente de la infraestructura
                codeResult = (int)HttpStatusCode.InternalServerError;
                var excep = (InternalServerErrorException)ex;
                globalException.Codigo = excep.Code;
                globalException.Message = excep.Message;
            }
            else
            {
                // Cualquier error no controlado
                codeResult = (int)HttpStatusCode.InternalServerError;
                globalException.Codigo = "INTERNAL_SERVER_ERROR";
                globalException.Message = "Ocurrio un error en el servidor";
            }

            _log.LogError("Exception: {@1} \n Message: {@2} \n Stacktrace: {@2}", JsonSerializer.Serialize(globalException), ex.Message, ex.StackTrace);

            context.Response.StatusCode = codeResult;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonSerializer.Serialize(globalException));
        }
    }
}
