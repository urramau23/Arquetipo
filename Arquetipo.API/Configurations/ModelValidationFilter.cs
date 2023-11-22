using Arquetipo.Core.Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace Arquetipo.API.Configurations
{
    public class ModelValidationFilter : IAsyncActionFilter
    {
        private readonly ILogger _log;
        public ModelValidationFilter(ILogger<ModelValidationFilter> log)
        {
            _log = log;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                _log.LogError("Excepcion controlada por ModelValidationFilter");

                GlobalException globalException = new GlobalException();
                globalException.Codigo = "PARAMETROS_INCORRECTOS";

                var Errores = (from item in context.ModelState
                               where item.Value.Errors.Any()
                               select new ErrorDetalle()
                               {
                                   idError = null,
                                   parametro = item.Key,
                                   mensaje = (from err in item.Value.Errors select err.ErrorMessage).First()
                               }).ToList();

                foreach (var error in Errores)
                {
                    globalException.Message += $"{error.parametro}->{error.mensaje}|";
                }

                _log.LogError(JsonSerializer.Serialize(globalException));
                context.Result = new BadRequestObjectResult(globalException);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            await next();
        }
    }

    public class ErrorDetalle
    {
        public string idError { get; set; }
        public string parametro { get; set; }
        public string mensaje { get; set;}
    }

}
