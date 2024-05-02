using NameSplitter.DTOs;
using System.Threading.Tasks;

namespace NameSplitter.Services
{
    public interface IApiClient
    {
        Task<ParseResponse> Parse( string input );
    }
}