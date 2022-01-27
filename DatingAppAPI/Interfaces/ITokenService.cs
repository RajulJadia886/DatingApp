using DatingAppAPI.Entities;
namespace DatingAppAPI.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user);
    }
}