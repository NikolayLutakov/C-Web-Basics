namespace Git
{
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Services;
    using System.Threading.Tasks;
    using MyWebServer;
    using MyWebServer.Controllers;
    using MyWebServer.Results.Views;

    public class Startup
    {
        public static async Task StartServer()
             => await HttpServer
                .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .WithServices(services => services
                    .Add<IViewEngine, CompilationViewEngine>()
                    .Add<IValidator, Validator>()
                    .Add<IPasswordHasher, PasswordHasher>()
                    .Add<ApplicationDbContext>())
                .WithConfiguration<ApplicationDbContext>(context => context
                    .Database.Migrate())
                .Start();
    }
}
