using Arquetipo.API.Configurations;
using Arquetipo.Core.Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Arquetipo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ModelValidationFilter))]
    public class DemoController : ControllerBase
    {
        [HttpGet]
        public IActionResult demo(int numero)
        {
            if (numero == 0)
                throw new BusinessException("Error Negocio simulado");

            if (numero == 1)
            {
                throw new InternalServerErrorException("Error Interno Servidor Simulado");
            }
            return Ok();
        }
    }
}
