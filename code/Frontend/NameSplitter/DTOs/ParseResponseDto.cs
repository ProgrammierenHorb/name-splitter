using NameSplitter.Enum;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NameSplitter.DTOs
{
    public class ParseResponseDto
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("structuredName")]
        public StructuredName StructuredName { get; set; }
    }

    public class StructuredName
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonIgnore]
        public GenderEnum Gender => _genderEnum;

        [JsonPropertyName("gender")]
        public string GenderString
        {
            get { return _genderString; }
            set
            {
                _genderString = value;

                switch( _genderString )
                {
                    case "MALE":
                        _genderEnum = GenderEnum.MALE;
                        break;

                    case "FEMALE":
                        _genderEnum = GenderEnum.FEMALE;
                        break;

                    case "DIVERSE":
                        _genderEnum = GenderEnum.DIVERSE;
                        break;
                }
            }
        }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("standardizedSalutation")]
        public string StandardizedSalutation { get; set; }

        [JsonPropertyName("titles")]
        public List<string> Titles { get; set; }

        private GenderEnum _genderEnum;
        private string _genderString;
    }
}