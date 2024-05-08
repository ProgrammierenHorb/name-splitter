﻿using NameSplitter.Enum;
using System.Text.Json.Serialization;

namespace NameSplitter.DTOs
{
    public class Title
    {
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

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("regex")]
        public string Regex { get; set; }

        private GenderEnum _genderEnum;

        private string _genderString;
    }
}