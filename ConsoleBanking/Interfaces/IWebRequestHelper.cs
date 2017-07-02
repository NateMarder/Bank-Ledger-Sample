using System.Threading.Tasks;
using ConsoleBanking.Models;

namespace ConsoleBanking.Interfaces
{
    public interface IWebRequestHelper
    {
        Task<int> UserSignIn( LoginFromConsoleViewModel model );
        Task<int> RegisterNewUser( LoginFromConsoleViewModel model );
    }
}