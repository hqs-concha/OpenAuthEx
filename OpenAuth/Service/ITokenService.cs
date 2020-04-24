

using System.Threading.Tasks;

namespace OpenAuth.Service
{
    public interface ITokenService
    {
        Task<string> GetAccessToken(string code);
        Task<string> RefreshToken(string token);
        Task Logout();
    }
}
