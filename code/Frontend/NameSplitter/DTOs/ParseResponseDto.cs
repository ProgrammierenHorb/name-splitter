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
        [JsonPropertyName("errorMessages")]
        public List<ErrorDto> ErrorMessages { get; set; }

        [JsonPropertyName("structuredName")]
        public StructuredName StructuredName { get; set; }
    }
}