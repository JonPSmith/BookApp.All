// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using BookApp.Books.Domain;
using Newtonsoft.Json;

namespace Test.Chapter16Listings
{
    public class DirectCosmosBook : CosmosBook
    {
        private DirectCosmosBook() : base() {}

        public DirectCosmosBook(int bookId, Book book)
        {
            if (book == null)
                return;

            BookId = bookId;
            Id = $"{Discriminator}|{bookId}";

            Title = book.Title;
            PublishedOn = book.PublishedOn;
            EstimatedDate = book.EstimatedDate;
            OrgPrice = book.OrgPrice;
            ActualPrice = book.ActualPrice;
        }

        public string Discriminator { get; set; } = "CosmosBook";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

}