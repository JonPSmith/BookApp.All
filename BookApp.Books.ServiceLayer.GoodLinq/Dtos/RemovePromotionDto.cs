// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using BookApp.Books.Domain;
using GenericServices;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Books.ServiceLayer.GoodLinq.Dtos
{
    public class RemovePromotionDto : ILinkToEntity<Book>
    {
        [HiddenInput]
        public int BookId { get; set; }

        public string Title { get; set; }
    }

}