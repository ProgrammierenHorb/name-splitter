using NameSplitter.DTOs;

namespace NameSplitter.Services
{
    public interface IApiClient
    {
        ParseResponse Parse( string input );
    }
}