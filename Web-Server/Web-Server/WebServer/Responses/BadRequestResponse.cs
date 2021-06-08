namespace WebServer.Responses
{
    using WebServer.Http;
    public class BadRequestResponse : HttpResponse
    {
        public BadRequestResponse() 
            : base(HttpStatusCode.BadRequest)
        {
        }
    }
}
