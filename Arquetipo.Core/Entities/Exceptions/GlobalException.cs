using System;

namespace Arquetipo.Core.Entities.Exceptions
{
    public class GlobalException
    {
        public string Status { get; set; } = "error";
        public string Message { get; set; } = "";
        public string Trx { get; set; } = Guid.NewGuid().ToString();
        public string Codigo { get; set; } = "ERROR_SIN_ESPECIFICAR";
    }
}
