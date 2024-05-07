using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NameSplitter.DTOs
{
    public class ErrorDto
    {
        public ErrorDto( string message )
        { Message = message; }

        [JsonPropertyName("endPos")]
        public int EndPos { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("startPos")]
        public int StartPos { get; set; }
    }
}