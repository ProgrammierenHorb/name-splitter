using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameSplitter.DTOs
{
    public class Title
    {
        public Title( string value )
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}