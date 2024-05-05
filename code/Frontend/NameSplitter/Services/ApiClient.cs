using NameSplitter.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NameSplitter.Services
{
    public class ApiClient: IApiClient
    {
        private readonly HttpClient _client;

        public ApiClient( HttpClient client )
        {
            _client = client;
            _client.BaseAddress = new Uri(@"http://localhost:8080/api/");
        }

        public async Task<List<string>> GetTitles()
        {
            try
            {
                var response = await _client.GetAsync($"getTitles");
                if( response.IsSuccessStatusCode )
                {
                    return JsonSerializer.Deserialize<List<string>>(await response.Content.ReadAsStringAsync());
                }

                return new List<string> { "Keine Titel verfügbar" };
            }
            catch( Exception )
            {
                return new List<string> { "Keine Titel verfügbar" };
            }
        }

        public async Task<ParseResponse> Parse( string input )
        {
            try
            {
                var response = await _client.GetAsync($"parse/{input}");
                if( response.IsSuccessStatusCode )
                {
                    return JsonSerializer.Deserialize<ParseResponse>(await response.Content.ReadAsStringAsync());
                }

                return new ParseResponse { Error = true, ErrorMessage = "Der eingegebene String konnte nicht geparsed werden!" };
            }
            catch( HttpRequestException ex )
            {
                return new ParseResponse { Error = true, ErrorMessage = ex.Message };
            }
            catch( Exception ex )
            {
                return new ParseResponse { Error = true, ErrorMessage = "Beim Parsen trat ein Fehler auf:" + ex.Message };
            }
        }
    }
}