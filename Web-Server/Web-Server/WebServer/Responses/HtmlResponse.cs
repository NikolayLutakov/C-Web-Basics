namespace WebServer.Responses
{
    using WebServer.Http;
    public class HtmlResponse : ContentResponse
    {
        public HtmlResponse(string html) 
            : base(html, HttpContentType.Html)
        {
        }
    }
}
