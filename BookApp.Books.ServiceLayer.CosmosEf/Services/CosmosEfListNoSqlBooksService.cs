// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.Books.Domain;
using BookApp.Books.Persistence.CosmosDb;
using BookApp.Books.ServiceLayer.CosmosEf.QueryObjects;
using BookApp.Books.ServiceLayer.DisplayCommon;
using BookApp.Common.Persistence.QueryObjects;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Books.ServiceLayer.CosmosEf.Services
{
    public class CosmosEfListNoSqlBooksService : ICosmosEfListNoSqlBooksService
    {
        private readonly CosmosDbContext _context;

        public CosmosEfListNoSqlBooksService(CosmosDbContext context)
        {
            _context = context;
        }

        public async Task<IList<CosmosBook>> SortFilterPageAsync(SortFilterPageOptionsNoCount options)
        {
            var booksFound = await _context.Books
                .AsNoTracking()                                             
                .OrderBooksBy(options.OrderByOptions)  
                .FilterBooksBy(options.FilterBy,       
                               options.FilterValue)
                .Page(options.PageNum - 1,options.PageSize)
                .ToListAsync();   

            options.SetupRestOfDto(booksFound.Count);

            var x = _context.ChangeTracker;

            return booksFound;
        }
    }

}