// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.Books.Domain;
using BookApp.Books.ServiceLayer.DisplayCommon;

namespace BookApp.Books.ServiceLayer.CosmosEf
{
    public interface ICosmosEfListNoSqlBooksService
    {
        Task<IList<CosmosBook>> SortFilterPageAsync(SortFilterPageOptionsNoCount options);
    }
}