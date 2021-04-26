﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BookApp.Books.Domain;
using BookApp.Books.Persistence.EfCoreSql;

namespace BookApp.Books.Infrastructure.Seeding
{
    public static class SeedDatabaseExtensions
    {
        private const string SummaryBookSearchName = "ManningBooks*.json";
        private const string DetailBookSearchName = "ManningDetails*.json";
        public const string SeedFileSubDirectory = "seedData";

        public static IEnumerable<Book> LoadManningBooks(this string wwwRootDir, bool tagAsOriginal)
        {
            var seedDirPath = Path.Combine(wwwRootDir, SeedFileSubDirectory);
            var loader = new ManningBookLoad(seedDirPath, SummaryBookSearchName, DetailBookSearchName);
            return loader.LoadBooks(tagAsOriginal);
        }

        public static async Task SeedDatabaseWithBooksAsync(this BookDbContext context, string wwwRootDir)
        {
            context.AddRange(wwwRootDir.LoadManningBooks(false));
            await context.SaveChangesAsync();
        }
    }
}