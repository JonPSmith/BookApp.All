// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using BookApp.Books.Domain;
using BookApp.Books.ServiceLayer.DisplayCommon;

namespace BookApp.Books.ServiceLayer.CosmosEf
{
    public class CosmosEfBookListCombinedDto
    {
        public CosmosEfBookListCombinedDto(SortFilterPageOptionsNoCount sortFilterPageData, IList<CosmosBook> booksList)
        {
            SortFilterPageData = sortFilterPageData;
            BooksList = booksList;
        }

        public SortFilterPageOptionsNoCount SortFilterPageData { get; private set; }

        public IList<CosmosBook> BooksList { get; private set; }
    }
}