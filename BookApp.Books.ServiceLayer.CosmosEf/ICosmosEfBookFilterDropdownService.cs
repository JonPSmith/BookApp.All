// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.Books.ServiceLayer.DisplayCommon;
using BookApp.Books.ServiceLayer.DisplayCommon.Dtos;

namespace BookApp.Books.ServiceLayer.CosmosEf
{
    public interface ICosmosEfBookFilterDropdownService
    {
        /// <summary>
        /// This makes the various Value + text to go in the dropdown based on the FilterBy option
        /// </summary>
        /// <param name="filterBy"></param>
        /// <returns></returns>
        Task<IEnumerable<DropdownTuple>> GetFilterDropDownValuesAsync(BooksFilterBy filterBy);
    }
}