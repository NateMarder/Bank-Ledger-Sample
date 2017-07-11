using System.Threading.Tasks;
using ConsoleBanking.Models;

namespace ConsoleBanking.Interfaces
{
    public interface IWebRequestHelper
    {
        Task<int> UserSignIn( LoginViewModel model );
        Task<int> RegisterNewUser( LoginViewModel model );
    }
}