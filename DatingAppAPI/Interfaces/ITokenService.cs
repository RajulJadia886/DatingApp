using System.Threading.Tasks;
using DatingAppAPI.Entities;
namespace DatingAppAPI.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(AppUser user);
    }
}