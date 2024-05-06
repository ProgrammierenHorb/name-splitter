using NameSplitter.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NameSplitter.DTOs
{
    /// <summary>
    /// Contrains the return values of the parse api call
    /// </summary>
    public class ParseResponseDto
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("structuredName")]
        public StructuredName StructuredName { get; set; }
    }
}