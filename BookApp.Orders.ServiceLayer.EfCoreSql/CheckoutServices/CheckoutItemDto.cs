// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace BookApp.Orders.ServiceLayer.EfCoreSql.CheckoutServices
{
    public class CheckoutItemDto
    {
        public int BookId { get; internal set; }

        public string Title { get; internal set; }

        public string AuthorsName { get; internal set; }

        public decimal BookPrice { get; internal set; }

        public string ImageUrl { get; internal set; }

        public short NumBooks { get; internal set; }
    }
}