using System.Threading.Tasks;

namespace DZzzz.OpenAPI.Core.Interfaces
{
    public interface ICommunicationService
    {
        Task<T> SendRequestAsync<T>(string url);
    }
}
