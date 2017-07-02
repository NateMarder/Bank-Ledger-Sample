using System.Threading.Tasks;

namespace ConsoleBanking.Interfaces
{
    public interface IWebRequestHelper
    {
        Task<string> TestRequestAsync();
    }
}