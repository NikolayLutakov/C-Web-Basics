namespace WebServer.App.Controllers
{
    using WebServer.Controllers;
    using WebServer.Http;

    public class CatsController : Controller
    {
        public CatsController(HttpRequest request)
            : base(request)
        {
        }

        public HttpResponse Create() => View();

        public HttpResponse Save()
        {
            var name = this.Request.Form["Name"];
            var age = this.Request.Form["Age"];

            return Text($"{name} - {age}");
        }
    }
}

