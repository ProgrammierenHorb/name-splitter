using NameSplitter.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace NameSplitter.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;

        public ApiClient( HttpClient client )
        {
            _client = client;
            _client.BaseAddress = new Uri(@"http://localhost:8080/api/");
        }

        public async Task<AddTitleResponse> AddTitle( string titleToAdd, bool useRegex )
        {
            try
            {
                var parameters = new
                {
                    titleToAdd,
                    useRegex
                };

                var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);

                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"addTitle", content);
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<AddTitleResponse>(await response.Content.ReadAsStringAsync());
                }

                return new AddTitleResponse { ErrorMessage = "Der eingegebene String konnte nicht geparsed werden!" };
            }
            catch (HttpRequestException ex)
            {
                return new AddTitleResponse { ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new AddTitleResponse { ErrorMessage = ex.Message };
            }
        }

        public async Task<List<string>> GetTitles()
        {
            try
            {
                var response = await _client.GetAsync($"getTitles");
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<List<string>>(await response.Content.ReadAsStringAsync());
                }

                return new List<string> { "Keine Titel verfügbar" };
            }
            catch (Exception)
            {
                return new List<string> { "Keine Titel verfügbar" };
            }
        }

        public async Task<ParseResponseDto> Parse( string input )
        {
            try
            {
                var response = await _client.GetAsync($"parse/{input}");
                if (response.IsSuccessStatusCode)
                {
                    var test = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ParseResponseDto>(await response.Content.ReadAsStringAsync());
                }

                return new ParseResponseDto { Error = true, ErrorMessage = "Der eingegebene String konnte nicht geparsed werden!" };
            }
            catch (HttpRequestException ex)
            {
                return new ParseResponseDto { Error = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ParseResponseDto { Error = true, ErrorMessage = "Beim Parsen trat ein Fehler auf:" + ex.Message };
            }
        }

        public async Task<bool> SaveNewTitle( string title )
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveParsedElement( StructuredName structuredName )
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"save", structuredName);
                if( response.IsSuccessStatusCode )
                {
                    return JsonSerializer.Deserialize<bool>(await response.Content.ReadAsStringAsync());
                }

                return false;
            }
            catch( Exception )
            {
                return false;
            }
        }
    }
}