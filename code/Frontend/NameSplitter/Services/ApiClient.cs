using NameSplitter.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameSplitter.Services
{
    public class ApiClient : IApiClient
    {
        public ApiClient()
        { }

        public ParseResponse Parse( string input )
        {
            return new ParseResponse();
        }
    }
}