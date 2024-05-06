using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NameSplitter.DTOs
{
    public class ParseResponse
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("structuredName")]
        public Structuredname StructuredName { get; set; }
    }

    public class Structuredname
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("standardizedSalutation")]
        public string StandardizedSalutation { get; set; }

        [JsonPropertyName("titles")]
        public List<string> Titles { get; set; }
    }
}