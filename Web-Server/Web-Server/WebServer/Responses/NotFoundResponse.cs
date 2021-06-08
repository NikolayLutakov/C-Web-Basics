namespace WebServer.Responses
{
    using WebServer.Http;
    public class NotFoundResponse : HttpResponse
    {
        public NotFoundResponse() 
            : base(HttpStatusCode.NotFound)
        {
        }
    }
}
