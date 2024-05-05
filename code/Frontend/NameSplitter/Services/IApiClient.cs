﻿using NameSplitter.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NameSplitter.Services
{
    public interface IApiClient
    {
        Task<List<string>> GetTitles();

        Task<ParseResponseDto> Parse( string input );

        Task<bool> SaveNewTitle( string title );

        Task<bool> SaveParsedElement( StructuredName structuredName );
    }
}