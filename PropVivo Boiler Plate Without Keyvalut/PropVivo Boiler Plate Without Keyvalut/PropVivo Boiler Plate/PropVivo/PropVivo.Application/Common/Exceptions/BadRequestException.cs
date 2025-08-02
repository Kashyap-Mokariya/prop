namespace PropVivo.Application.Common.Exceptions
{
    public sealed class BadRequestException : ApplicationException
    {
        public BadRequestException(string message)
            : base("Bad Request", message)
        {
        }
    }
}