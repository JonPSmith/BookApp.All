// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using BookApp.Books.Persistence.EfCoreSql;
using BookApp.Books.ServiceLayer.CachedSql.QueryObjects;
using BookApp.Books.ServiceLayer.DisplayCommon;
using BookApp.Books.ServiceLayer.DisplayCommon.Dtos;
using BookApp.Common.Persistence.QueryObjects;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Books.ServiceLayer.CachedSql.Services
{
    public class ListBooksCachedNoCountService : IListBooksCachedNoCountService
    {
        private readonly BookDbContext _context;

        public ListBooksCachedNoCountService(BookDbContext context)
        {
            _context = context;
        }

        public IQueryable<BookListDto> SortFilterPage
            (SortFilterPageOptionsNoCount options)
        {
            var booksQuery = _context.Books
                .AsNoTracking()
                .MapBookCachedToDto()
                .OrderBooksBy(options.OrderByOptions)
                .FilterBooksBy(options.FilterBy, options.FilterValue)
                .Page(options.PageNum - 1, options.PageSize);

            return booksQuery;
        }
    }


}