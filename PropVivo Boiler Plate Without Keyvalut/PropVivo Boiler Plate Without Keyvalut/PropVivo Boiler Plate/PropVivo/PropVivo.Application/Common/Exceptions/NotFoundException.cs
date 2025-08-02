namespace PropVivo.Application.Common.Exceptions
{
    public sealed class NotFoundException : ApplicationException
    {
        public NotFoundException(string message)
            : base("Not Found", message)
        {
        }
    }
}