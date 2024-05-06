using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NameSplitter.DTOs
{
    /// <summary>
    /// Contrains the return value of the add title api call
    /// </summary>
    public class AddTitleResponse
    {
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}