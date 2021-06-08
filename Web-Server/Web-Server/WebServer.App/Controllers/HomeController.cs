namespace WebServer.App.Controllers
{
    using WebServer.Controllers;
    using WebServer.Http;

    public class HomeController : Controller
    {
        public HomeController(HttpRequest request) 
            : base(request)
        {
        }

        public HttpResponse Index() => Text("Hello from home!");

        public HttpResponse LocalRedirect() => Redirect("/Cats");

        public HttpResponse ToSoftUni() => Redirect("https://softuni.bg");
    }
}
