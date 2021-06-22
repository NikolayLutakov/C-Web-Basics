using System.Threading.Tasks;

namespace BattleCards
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await Startup.StartServer();
        }
    }
}
