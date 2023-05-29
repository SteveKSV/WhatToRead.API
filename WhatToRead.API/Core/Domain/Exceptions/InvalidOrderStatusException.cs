namespace Core.Domain.Exceptions
{
    public class InvalidOrderStatusException : Exception
    {
        public InvalidOrderStatusException(string message) : base(message)
        {
        }

        public InvalidOrderStatusException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
