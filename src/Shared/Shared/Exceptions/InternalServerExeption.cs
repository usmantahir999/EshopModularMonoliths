namespace Shared.Exceptions
{
    public class InternalServerExeption : Exception
    {
        public InternalServerExeption(string message): base(message)
        {
            
        }
        public InternalServerExeption(string message, string details) : base(message)
        {
            Details = details;
        }

        public string? Details { get; }
    }
}
