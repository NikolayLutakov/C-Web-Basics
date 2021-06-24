using CarShop.Data;
using System.Linq;

namespace CarShop.Services
{
    public class UsersService: IUsersService
    {
        private readonly ApplicationDbContext data;

        public UsersService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public bool IsUserMechanic(string userId)
            => this.data.Users.Where(x => x.Id == userId && x.IsMechanic).FirstOrDefault() != null ? true : false;    
    }
}
