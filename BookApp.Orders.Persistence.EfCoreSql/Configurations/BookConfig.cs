// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using BookApp.Orders.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookApp.Orders.Persistence.EfCoreSql.Configurations
{
    internal class BookViewConfig : IEntityTypeConfiguration<BookView>
    {
        public void Configure(EntityTypeBuilder<BookView> entity)
        {
            entity.ToView("Books");   //Map the BookView to the Books table in the BookDbContext part
        }
    }
}