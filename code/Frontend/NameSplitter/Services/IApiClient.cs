using NameSplitter.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NameSplitter.Services
{
    /// <summary>
    /// Api for communication with the backend
    /// </summary>
    public interface IApiClient
    {
        /// <summary>
        /// Saves a new title
        /// </summary>
        /// <param name="titleToAdd"></param>
        /// <param name="regex"></param>
        /// <returns> True if successful</returns>
        Task<bool> AddTitle( string titleToAdd, string regex );

        /// <summary>
        /// Gets all titles
        /// </summary>
        /// <returns>List of titles as string</returns>
        Task<List<string>> GetTitles();

        /// <summary>
        /// Parses the input
        /// </summary>
        /// <param name="input"></param>
        /// <returns>ParseResponseDto</returns>
        Task<ParseResponseDto> Parse( string input );

        Task<bool> RemoveTitle( string title );

        /// <summary>
        /// Saves a parsed entity
        /// </summary>
        /// <param name="structuredName"></param>
        /// <returns> True if successful</returns>
        Task<StructuredName> SaveParsedElement( StructuredName structuredName );
    }
}