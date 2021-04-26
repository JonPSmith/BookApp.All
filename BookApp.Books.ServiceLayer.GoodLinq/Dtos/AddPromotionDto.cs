// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using BookApp.Books.Domain;
using GenericServices;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Books.ServiceLayer.GoodLinq.Dtos
{
    public class AddPromotionDto : ILinkToEntity<Book>
    {
        [HiddenInput]
        public int BookId { get; set; }

        public string Title { get; set; }

        public decimal OrgPrice { get; set; }
        public decimal ActualPrice { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string PromotionalText { get; set; }
    }

}