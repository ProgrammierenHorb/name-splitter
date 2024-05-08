using NameSplitter.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace NameSplitter.Services
{
    /// <summary>
    /// Implements the methodes of IApiClient
    /// </summary>
    /// <seealso cref="NameSplitter.Services.IApiClient" />
    public class ApiClient: IApiClient
    {
        private readonly HttpClient _client;

        public ApiClient( HttpClient client )
        {
            _client = client;
            _client.BaseAddress = new Uri(@"http://localhost:8080/api/");
        }

        /// <summary>
        /// Adds the title and its regex in the backend.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="regex">The regex.</param>
        /// <returns></returns>
        public async Task<bool> AddTitle( string name, string regex )
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { name, regex });

                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"addTitle", content);
                if( response.IsSuccessStatusCode )
                    return JsonSerializer.Deserialize<bool>(await response.Content.ReadAsStringAsync());

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all titles which are hold in the backend
        /// </summary>
        /// <returns>
        /// List of titles as string
        /// </returns>
        public async Task<List<Title>> GetTitles()
        {
            try
            {
                var response = await _client.GetAsync($"getTitles");
                if( response.IsSuccessStatusCode )
                    return JsonSerializer.Deserialize<List<Title>>(await response.Content.ReadAsStringAsync());

                return new List<Title> { new Title { Name = "Keine Titel verfügbar" } };
            }
            catch( Exception )
            {
                return new List<Title> { new Title { Name = "Keine Titel verfügbar" } };
            }
        }

        /// <summary>
        /// Calls the backend with the given input to parse
        /// the input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>
        /// ParseResponseDto with all found elemnts splitted
        /// </returns>
        public async Task<ParseResponseDto> Parse( string input )
        {
            try
            {
                var content = new StringContent(input, System.Text.Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"parse", content);
                if( response.IsSuccessStatusCode )
                    return JsonSerializer.Deserialize<ParseResponseDto>(await response.Content.ReadAsStringAsync());

                return new ParseResponseDto { ErrorMessages = new List<ErrorDto> { new ErrorDto("Der eingegebene String konnte nicht geparsed werden!") } };
            }
            catch( HttpRequestException ex )
            {
                return new ParseResponseDto { ErrorMessages = new List<ErrorDto> { new ErrorDto(ex.Message) } };
            }
            catch( Exception ex )
            {
                return new ParseResponseDto { ErrorMessages = new List<ErrorDto> { new ErrorDto("Beim Parsen trat ein Fehler auf:" + ex.Message) } };
            }
        }

        /// <summary>
        /// Removes a title which is provided by the backend.
        /// After calling this methode, the backend deletes it.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>bool if it was removed successfully</returns>
        public async Task<bool> RemoveTitle( Title title )
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(title);

                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"removeTitle", content);
                if( response.IsSuccessStatusCode )
                    return JsonSerializer.Deserialize<bool>(await response.Content.ReadAsStringAsync());

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Calls the backend and gets not only the
        /// splitted elements but also the standardized salutaion
        /// </summary>
        /// <param name="structuredName"></param>
        /// <returns>
        /// True if successful
        /// </returns>
        public async Task<StructuredName> SaveParsedElement( StructuredName structuredName )
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"save", structuredName);
                if( response.IsSuccessStatusCode )
                {
                    var structuredNameResponse = JsonSerializer.Deserialize<StructuredName>(await response.Content.ReadAsStringAsync());
                    structuredNameResponse.Key = structuredName.Key;
                    return structuredNameResponse;
                }

                return null;
            }
            catch( Exception )
            {
                return null;
            }
        }
    }
}