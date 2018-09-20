using System.Threading.Tasks;
using Products.Models;

namespace Products.Services
{
    public interface IApiService
    {
        Task<Response> CheckConnection();
        Task<Response> Delete<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, T model);
        Task<Response> Get<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, int id);
        Task<Response> GetList<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken);
        Task<Response> GetList<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, int id);
        Task<TokenResponse> GetToken(string urlBase, string username, string password);
        Task<Response> Post<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, T model);
        Task<Response> Post<T>(string urlBase, string servicePrefix, string controller, T model);
        Task<Response> Put<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, T model);
    }
}