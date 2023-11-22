namespace Arquetipo.Core.Entities.Exceptions
{
    public class BusinessException : System.Exception
    {
        public string Code { get; set; }
        public BusinessException(string message) : base(message) { 
            Code = "BUSINESS_EXCEPTION";
        }
        public BusinessException(string message, string code) : base(message) 
        { 
            Code = code;
        }
    }
}
