using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameSplitter.DTOs
{
    public class ParseResponse
    {
        public string Title { get; set; } = "";

        public string Salutation { get; set; } = "";

        public string StandardizedLetterSalutation { get; set; } = "";

        public string Gender { get; set; } = "";

        public string Firstname { get; set; } = "";

        public string Surname { get; set; } = "";
    }
}