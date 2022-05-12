namespace RealEstate.Application.Exceptions
{
    public class HttpRequestFailException : Exception
    {
        public HttpRequestFailException(string? message) : base(message)
        {
        }
    }
}
