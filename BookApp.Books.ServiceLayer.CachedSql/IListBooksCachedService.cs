// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using BookApp.Books.ServiceLayer.DisplayCommon;
using BookApp.Books.ServiceLayer.DisplayCommon.Dtos;

namespace BookApp.Books.ServiceLayer.CachedSql
{
    public interface IListBooksCachedService
    {
        Task<IQueryable<BookListDto>> SortFilterPageAsync
            (SortFilterPageOptions options);
    }
}