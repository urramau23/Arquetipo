namespace Arquetipo.Core.Entities.Exceptions
{
    public class NotFoundException : System.Exception
    {
        public string Code { get; set; }
        public NotFoundException(string message) : base(message) {
            Code = "RECURSO_NO_ENCONTRADO";
        }
        public NotFoundException(string message, string code) : base(message)
        {
            Code = code;
        }
    }

}
