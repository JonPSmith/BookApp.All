﻿// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.Books.Domain;
using BookApp.Books.Infrastructure.CachedValues.CheckFixCode;
using BookApp.Books.Persistence.EfCoreSql;
using BookApp.Persistence.EfCoreSql.Books;
using Microsoft.Extensions.Logging;
using Test.TestHelpers;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests.TestPersistenceSqlBooks
{
    public class TestCheckFixCachedValues
    {

        [Fact]
        public async Task TestCheckFixCacheValuesServiceFindFixReviewsOk()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using var context = new BookDbContext(options);
            context.Database.EnsureCreated();

            var books = context.SeedDatabaseFourBooks(); //The Review cache values will be incorrect, and createUpdate not set
            SetAllBooksAsUpdatedNow(books);
            books[3].UpdateReviewCachedValues(2, books[3].Reviews.Average(x => x.NumStars));
            await context.SaveChangesAsync();

            var logs = new List<LogOutput>();
            var logger = new Logger<CheckFixCacheValuesService>(new LoggerFactory(new[] { new MyLoggerProviderActionOut(logs.Add) }));
            var service = new CheckFixCacheValuesService(context, logger);

            //ATTEMPT
            var notes = await service.RunCheckAsync(new DateTime(2000, 1, 1), true, default);

            //VERIFY
            logs.Count.ShouldEqual(0);
            notes.First().ShouldEqual("Looked at 4 SQL books and found no errors");
        }

        [Fact]
        public async Task TestCheckFixCacheValuesServiceFindFixBadReviews()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using var context = new BookDbContext(options);
            context.Database.EnsureCreated();

            var books = context.SeedDatabaseFourBooks(); //The Review cache values will be incorrect, and createUpdate not set
            SetAllBooksAsUpdatedNow(books);
            await context.SaveChangesAsync();

            var logs = new List<LogOutput>();
            var logger = new Logger<CheckFixCacheValuesService>(new LoggerFactory(new[] { new MyLoggerProviderActionOut(logs.Add) }));
            var service = new CheckFixCacheValuesService(context,  logger);

            //ATTEMPT
            var notes = await service.RunCheckAsync(new DateTime(2000, 1, 1), true, default);

            //VERIFY
            context.ChangeTracker.Clear();
            var readBook = context.Books.Single(x => x.BookId == books[3].BookId);
            readBook.ReviewsAverageVotes.ShouldEqual(4);
            readBook.ReviewsCount.ShouldEqual(2);

            logs.Count.ShouldEqual(1);
            notes.First().ShouldEqual("Looked at 4 SQL books and found 1 errors and fixed them");
            notes[1].ShouldEqual("BookId: 4");
        }

        [Fact]
        public async Task TestCheckFixCacheValuesServiceFindBadOnlyReviews()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using var context = new BookDbContext(options);
            context.Database.EnsureCreated();

            var books = context.SeedDatabaseFourBooks(); //The Review cache values will be incorrect, and createUpdate not set
            SetAllBooksAsUpdatedNow(books);
            await context.SaveChangesAsync();

            var logs = new List<LogOutput>();
            var logger = new Logger<CheckFixCacheValuesService>(new LoggerFactory(new[] { new MyLoggerProviderActionOut(logs.Add) }));
            var service = new CheckFixCacheValuesService(context, logger);

            //ATTEMPT
            var notes = await service.RunCheckAsync(new DateTime(2000, 1, 1), false, default);

            //VERIFY
            context.ChangeTracker.Clear();
            var readBook = context.Books.Single(x => x.BookId == books[3].BookId);
            readBook.ReviewsAverageVotes.ShouldEqual(0);
            readBook.ReviewsCount.ShouldEqual(0);

            logs.Count.ShouldEqual(1);
            notes.First().ShouldEqual("Looked at 4 SQL books and found 1 errors (not fixed)");
            notes[1].ShouldEqual("BookId: 4");
        }

        [Fact]
        public async Task TestCheckFixCacheValuesServiceFindFixName()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using var context = new BookDbContext(options);
            context.Database.EnsureCreated();

            var books = context.SeedDatabaseFourBooks(); //The Review cache values will be incorrect, and createUpdate not set
            SetAllBooksAsUpdatedNow(books.Take(2));
            books[0].AuthorsLink.Single().Author.Name = "New Name";
            await context.SaveChangesAsync();

            var logs = new List<LogOutput>();
            var logger = new Logger<CheckFixCacheValuesService>(new LoggerFactory(new[] { new MyLoggerProviderActionOut(logs.Add) }));
            var service = new CheckFixCacheValuesService(context, logger);

            //ATTEMPT
            var notes = await service.RunCheckAsync(new DateTime(2000, 1, 1), true, default);

            //VERIFY
            context.ChangeTracker.Clear();
            var readBooks = context.Books.ToList();
            readBooks[0].AuthorsOrdered.ShouldEqual("New Name");
            readBooks[1].AuthorsOrdered.ShouldEqual("New Name");

            logs.Count.ShouldEqual(2);
            notes.First().ShouldEqual("Looked at 2 SQL books and found 2 errors and fixed them");
            notes[1].ShouldEqual("BookId: 1");
            notes[3].ShouldEqual("BookId: 2");
        }

        [Fact]
        public async Task TestCheckFixCacheValuesServiceFindOnlyName()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using var context = new BookDbContext(options);
            context.Database.EnsureCreated();

            var books = context.SeedDatabaseFourBooks(); //The Review cache values will be incorrect, and createUpdate not set
            SetAllBooksAsUpdatedNow(books.Take(2));
            books[0].AuthorsLink.Single().Author.Name = "New Name";
            await context.SaveChangesAsync();

            var logs = new List<LogOutput>();
            var logger = new Logger<CheckFixCacheValuesService>(new LoggerFactory(new[] { new MyLoggerProviderActionOut(logs.Add) }));
            var service = new CheckFixCacheValuesService(context, logger);

            //ATTEMPT
            var notes = await service.RunCheckAsync(new DateTime(2000, 1, 1), false, default);

            //VERIFY
            context.ChangeTracker.Clear();
            var readBooks = context.Books.ToList();
            readBooks[0].AuthorsOrdered.ShouldEqual("Martin Fowler");
            readBooks[1].AuthorsOrdered.ShouldEqual("Martin Fowler");

            logs.Count.ShouldEqual(2);
            notes.First().ShouldEqual("Looked at 2 SQL books and found 2 errors (not fixed)");
            notes[1].ShouldEqual("BookId: 1");
            notes[3].ShouldEqual("BookId: 2");
        }

        private void SetAllBooksAsUpdatedNow(IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                book.LogAddUpdate(false);
            }
        }
    }
}