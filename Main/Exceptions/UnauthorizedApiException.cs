namespace Main.Exceptions
{
    public class UnauthorizedApiException : HttpResponseException
    {
        public UnauthorizedApiException() : base(401)
        {
            ResponseData = new Dictionary<string, string>
            {
                { "Detail", "Authorization token is expired or not provided." }
            };
        }
    }
}
