﻿using NameSplitter.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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

        public async Task<bool> AddTitle( string name, string regex )
        {
            try
            {
                var parameters = new
                {
                    name,
                    regex
                };

                var jsonContent = JsonSerializer.Serialize(parameters);

                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"addTitle", content);
                if( response.IsSuccessStatusCode )
                {
                    return JsonSerializer.Deserialize<bool>(await response.Content.ReadAsStringAsync());
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<string>> GetTitles()
        {
            try
            {
                var response = await _client.GetAsync($"getTitles");
                if( response.IsSuccessStatusCode )
                    return JsonSerializer.Deserialize<List<string>>(await response.Content.ReadAsStringAsync());

                return new List<string> { "Keine Titel verfügbar" };
            }
            catch( Exception )
            {
                return new List<string> { "Keine Titel verfügbar" };
            }
        }

        public async Task<ParseResponseDto> Parse( string input )
        {
            try
            {
                var response = await _client.GetAsync($"parse/{input}");
                if( response.IsSuccessStatusCode )
                    return JsonSerializer.Deserialize<ParseResponseDto>(await response.Content.ReadAsStringAsync());

                return new ParseResponseDto { Error = true, ErrorMessage = "Der eingegebene String konnte nicht geparsed werden!" };
            }
            catch( HttpRequestException ex )
            {
                return new ParseResponseDto { Error = true, ErrorMessage = ex.Message };
            }
            catch( Exception ex )
            {
                return new ParseResponseDto { Error = true, ErrorMessage = "Beim Parsen trat ein Fehler auf:" + ex.Message };
            }
        }

        public async Task<StructuredName> SaveParsedElement( StructuredName structuredName )
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"save", structuredName);
                if( response.IsSuccessStatusCode )
                {
                    var structuredNameResponse = JsonSerializer.Deserialize<StructuredName>(await response.Content.ReadAsStringAsync());
                    structuredNameResponse.Key = Guid.NewGuid();
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