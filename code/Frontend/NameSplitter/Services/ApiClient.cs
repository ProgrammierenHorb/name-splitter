﻿using NameSplitter.DTOs;
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

        public async Task<ParseResponse> Parse( string input )
        {
            try
            {
                var response = await _client.GetAsync($"parse/{input}");
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<ParseResponse>(await response.Content.ReadAsStringAsync());
                }

                return new ParseResponse { Error = true, ErrorMessage = "Der eingegebene String konnte nicht geparsed werden!" };
            }
            catch (HttpRequestException ex)
            {
                return new ParseResponse { Error = true, ErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ParseResponse { Error = true, ErrorMessage = "Beim Parsen trat ein Fehler auf:" + ex.Message };
            }
        }
    }
}