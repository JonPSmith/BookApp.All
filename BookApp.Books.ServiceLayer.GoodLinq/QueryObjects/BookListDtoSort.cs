// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using BookApp.Books.ServiceLayer.DisplayCommon;
using BookApp.Books.ServiceLayer.DisplayCommon.Dtos;

namespace BookApp.Books.ServiceLayer.GoodLinq.QueryObjects
{

    public static class BookListDtoSort
    {
        public static IQueryable<BookListDto> OrderBooksBy
            (this IQueryable<BookListDto> books, OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder: 
                    return books.OrderByDescending( 
                        x => x.BookId); 
                case OrderByOptions.ByVotes: 
                    return books.OrderByDescending(x => 
                        x.ReviewsAverageVotes); 
                case OrderByOptions.ByPublicationDate: 
                    return books.OrderByDescending( 
                        x => x.PublishedOn); 
                case OrderByOptions.ByPriceLowestFirst: 
                    return books.OrderBy(x => x.ActualPrice); 
                case OrderByOptions.ByPriceHighestFirst: 
                    return books.OrderByDescending( 
                        x => x.ActualPrice); 
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}