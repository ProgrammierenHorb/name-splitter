using System.Text.Json.Serialization;

namespace NameSplitter.DTOs
{
    /// <summary>
    /// Contains properties for returning a specific error
    /// </summary>
    public class ErrorDto
    {
        [JsonPropertyName("endPos")]
        public int EndPos { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("startPos")]
        public int StartPos { get; set; }

        public ErrorDto( string message )
        { Message = message; }
    }
}