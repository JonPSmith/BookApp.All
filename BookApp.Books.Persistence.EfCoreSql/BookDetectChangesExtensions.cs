// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using BookApp.Books.Domain.SupportTypes;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Books.Persistence.EfCoreSql
{
    public static class BookDetectChangesExtensions
    {
        public static void ChangeChecker(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries()
                .Where(e =>                          
                     e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                var tracked = entry.Entity as ICreatedUpdated;         //#D
                tracked?.LogAddUpdate(entry.State == EntityState.Added);
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(ICreatedUpdated.LastUpdatedUtc))  
                        .IsModified = true;
                }
            }
        }
    }
}