namespace WebServer.Responses
{
    using WebServer.Http;
    public class TextResponse : ContentResponse
    {
        public TextResponse(string text) 
            : base(text, HttpContentType.PlainText)
        {
        }
    }
}
