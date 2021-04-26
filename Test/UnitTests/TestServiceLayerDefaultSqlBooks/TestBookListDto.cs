﻿// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using BookApp.Books.Persistence.EfCoreSql;
using BookApp.Books.ServiceLayer.DisplayCommon.Dtos;
using BookApp.Books.ServiceLayer.GoodLinq.QueryObjects;
using BookApp.Persistence.EfCoreSql.Books;
using Microsoft.EntityFrameworkCore;
using Test.TestHelpers;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests.TestServiceLayerDefaultSqlBooks
{
    public class TestBookListDto
    {
        public TestBookListDto(ITestOutputHelper output)
        {
            _output = output;
        }

        private readonly ITestOutputHelper _output;


        [Fact]
        public void TestAverageDirectOk()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using var context = new BookDbContext(options);
            context.Database.EnsureCreated();
            context.SeedDatabaseFourBooks();
            context.ChangeTracker.Clear();

            //ATTEMPT
            var query = context.Books.Select(p =>
                !p.Reviews.Any()
                    ? null
                    : (decimal?) p.Reviews.Select(q => q.NumStars).Average());
            _output.WriteLine(query.ToQueryString());
            var averages = query.ToList();

            //VERIFY
            averages.Count(x => x == null).ShouldEqual(3);
            averages.Count(x => x != null).ShouldEqual(1);
        }

        [Fact]
        public void TestAverageOk()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using (var context = new BookDbContext(options))
            {
                context.Database.EnsureCreated();
                context.SeedDatabaseFourBooks();

                //ATTEMPT
                var dtos = context.Books.Select(p => new
                {
                    NumReviews = p.Reviews.Count(),
                    ReviewsAverageVotes = !p.Reviews.Any() ? null : (double?)p.Reviews.Average(q => q.NumStars)
                }).ToList();

                //VERIFY
                dtos.Any(x => x.ReviewsAverageVotes > 0).ShouldBeTrue();
            }
        }

        [Fact]
        public void TestDirectSelectBookListDtoOk()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using (var context = new BookDbContext(options))
            {
                context.Database.EnsureCreated();
                context.SeedDatabaseFourBooks();

                //ATTEMPT
                var dtos = context.Books.Select(p => new BookListDto
                {
                    BookId = p.BookId,
                    Title = p.Title,
                    OrgPrice = p.ActualPrice,
                    PublishedOn = p.PublishedOn,
                }).ToList();

                //VERIFY
                dtos.Last().BookId.ShouldNotEqual(0);
                dtos.Last().Title.ShouldNotBeNull();
                dtos.Last().OrgPrice.ShouldNotEqual(0);
            }
        }

        [Fact]
        public void TestEagerBookListDtoOk()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using var context = new BookDbContext(options);
            context.Database.EnsureCreated();
            context.SeedDatabaseFourBooks();
            context.ChangeTracker.Clear();

            //ATTEMPT
            var firstBook = context.Books
                .Include(r => r.AuthorsLink)
                .ThenInclude(r => r.Author)
                .Include(r => r.Reviews)
                .First();
            var dto = new BookListDto
            {
                BookId = firstBook.BookId,
                Title = firstBook.Title,
                OrgPrice = firstBook.OrgPrice,
                ActualPrice = firstBook.ActualPrice,
                PromotionText = firstBook.PromotionalText, 
                AuthorsOrdered = string.Join(", ", 
                    firstBook.AuthorsLink 
                        .OrderBy(l => l.Order) 
                        .Select(a => a.Author.Name)), 
                ReviewsCount = firstBook.Reviews.Count(), 
                //The test on there being any reviews is needed because of bug in EF Core V2.0.0, issue #9516
                ReviewsAverageVotes = 
                    firstBook.Reviews.Count() == 0 
                        ? null 
                        : (double?)firstBook.Reviews 
                            .Average(q => q.NumStars) 
            };

            //VERIFY
            dto.BookId.ShouldNotEqual(0);
            dto.Title.ShouldNotBeNull();
            dto.OrgPrice.ShouldNotEqual(0);
            dto.AuthorsOrdered.ShouldNotBeNull();
            /*********************************************************
                #A Notice the use of ?. This returns null if Promotion is null, otherwise it returns the property
                #B This orders the authors' names by the Order and then extracts their names
                #C We simply count the number of reviews
                #D EF Core turns the LINQ average into the SQL AVG command that runs on the database
                * *******************************************************/
        }

        [Fact]
        public void TestIndividualBookListDtoOk()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using var context = new BookDbContext(options);
            context.Database.EnsureCreated();
            context.SeedDatabaseFourBooks();
            context.ChangeTracker.Clear();

            //ATTEMPT
            var titles = context.Books.Select(p => p.Title);
            var orgPrices = context.Books.Select(p => p.OrgPrice);
            var actualPrices = context.Books.Select(p => p.ActualPrice);
            var pText = context.Books.Select(p => p.PromotionalText);
            var authorOrdered =
                context.Books.Select(p =>
                    string.Join(", ",
                        p.AuthorsLink
                            .OrderBy(q => q.Order)
                            .Select(q => q.Author.Name)));
            var reviewsCount = context.Books.Select(p => p.Reviews.Count());
            //The test on there being any reviews is needed because of bug in EF Core V2.0.0, issue #9516
            var reviewsAverageVotes = context.Books.Select(p =>
                p.Reviews.Count() == 0
                    ? null
                    : (double?)p.Reviews.Average(q => q.NumStars));

            //VERIFY
            titles.ToList();
            orgPrices.ToList();
            actualPrices.ToList();
            pText.ToList();
            authorOrdered.ToList();
            reviewsCount.ToList();
            reviewsAverageVotes.ToList();
        }

        [Fact]
        public void TestIQueryableSelectBookListDtoOk()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using (var context = new BookDbContext(options))
            {
                context.Database.EnsureCreated();
                var books = context.SeedDatabaseFourBooks();

                //ATTEMPT
                var dtos = context.Books.MapBookToDto().OrderByDescending(x => x.BookId).ToList();

                //VERIFY
                dtos.First().BookId.ShouldNotEqual(0);
                dtos.First().Title.ShouldNotBeNull();
                dtos.First().OrgPrice.ShouldNotEqual(0);
                dtos.First().ActualPrice.ShouldNotEqual(dtos.Last().OrgPrice);
                dtos.First().AuthorsOrdered.Length.ShouldBeInRange(1, 100);
                dtos.First().ReviewsCount.ShouldEqual(2);
                dtos.First().ReviewsAverageVotes.ShouldEqual(4);
            }
        }

        [Fact]
        public void TestRawSqlOk()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<BookDbContext>();
            using (var context = new BookDbContext(options))
            {
                context.Database.EnsureCreated();
                context.SeedDatabaseFourBooks();

                //ATTEMPT
                var books =
                    context.Books.FromSqlRaw(
                            "SELECT * FROM Books AS a ORDER BY (SELECT AVG(b.NumStars) FROM Review AS b WHERE b.BookId = a.BookId) DESC")
                        .ToList();

                //VERIFY
                books.First().Title.ShouldEqual("Quantum Networking");
            }
        }
    }
}