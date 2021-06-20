using MyWebServer.Controllers;
using MyWebServer.Http;

namespace CarShop.Controllers
{
    public class HomeController : Controller
    {
        public HttpResponse Index()
        {              
            if (this.User.IsAuthenticated)
            {
                return this.Redirect("/Cars/All");
            }

            return this.View();
        }
    }
}
