// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using BookApp.Books.Domain;
using GenericServices;

namespace BookApp.Books.ServiceLayer.GoodLinq.Dtos
{
    public class SimpleBookList : ILinkToEntity<Book>
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
    }
}