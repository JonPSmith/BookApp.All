// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using BookApp.Books.ServiceLayer.DisplayCommon;
using BookApp.Books.ServiceLayer.DisplayCommon.Dtos;

namespace BookApp.Books.ServiceLayer.CachedSql
{
    public interface IListBooksCachedNoCountService
    {
        IQueryable<BookListDto> SortFilterPage
            (SortFilterPageOptionsNoCount options);
    }
}