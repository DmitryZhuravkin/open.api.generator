using System.Threading.Tasks;

namespace DZzzz.Swag.Generator.Core.Interfaces
{
    public interface ICommunicationService
    {
        Task<T> SendRequestAsync<T>(string url);
    }
}