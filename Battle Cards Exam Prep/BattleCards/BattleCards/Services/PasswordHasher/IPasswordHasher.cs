namespace BattleCards.Services.PasswordHasher
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
