using System;

namespace Arquetipo.Core.Entities.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public string Code { get; set; }
        public InternalServerErrorException(string message) : base(message)
        {
            Code = "ERROR_INTERNO_DEL_SERVIDOR";
        }
        public InternalServerErrorException(string message, string code) : base(message)
        {
            Code = code;
        }
    }

}
