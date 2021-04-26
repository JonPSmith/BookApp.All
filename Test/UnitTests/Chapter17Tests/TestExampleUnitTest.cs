﻿// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using BookApp.Books.Persistence.EfCoreSql;
using BookApp.Persistence.EfCoreSql.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Test.TestHelpers;
using TestSupport.EfHelpers;
using TestSupport.Helpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests.Chapter17Tests
{
    public class TestExampleUnitTest
    {
        private readonly ITestOutputHelper _output;

        public TestExampleUnitTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestExample()
        {
            //SETUP
            var options = SqliteInMemory
                .CreateOptions<BookDbContext>();

            using var context = new BookDbContext(options);

            context.Database.EnsureCreated();

            context.SeedDatabaseFourBooks();

            //ATTEMPT
            var testDate = new DateTime(2020, 1, 1);
            var query = context.Books
                .Where(x => x.PublishedOn < testDate);

            var books = query.ToList();

            //VERIFY
            _output.WriteLine(query.ToQueryString());
            books.Count.ShouldEqual(3);
        }

        [Fact]
        public void TestExampleSqlServerEnsureDeletedEnsureCreated()
        {
            //SETUP
            var options = this.CreateUniqueClassOptions<BookDbContext>();

            using var context = new BookDbContext(options);

            using (new TimeThings(_output, "EnsureDeleted/EnsureCreated"))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            context.SeedDatabaseFourBooks();

            //ATTEMPT
            var testDate = new DateTime(2020, 1, 1);
            var query = context.Books
                .Where(x => x.PublishedOn < testDate);

            var books = query.ToList();

            //VERIFY
            books.Count.ShouldEqual(3);
        }

        [Fact]
        public void TestExampleSqlServerEnsureDeletedEnsureCreatedAgain()
        {
            TestExampleSqlServerEnsureDeletedEnsureCreated();
        }

        [Fact]
        public void TestExampleSqlServerEnsureDeletedEnsureCreatedAgainAgain()
        {
            TestExampleSqlServerEnsureDeletedEnsureCreated();
        }

        [Fact]
        public void TestExampleSqlServerEnsureClean()
        {
            //SETUP
            var options = this.
                CreateUniqueClassOptions<BookDbContext>();

            using var context = new BookDbContext(options);

            using (new TimeThings(_output, "EnsureClean"))
            {
                context.Database.EnsureClean();
            }

            context.SeedDatabaseFourBooks();

            //ATTEMPT
            var testDate = new DateTime(2020, 1, 1);
            var query = context.Books
                .Where(x => x.PublishedOn < testDate);

            var books = query.ToList();

            //VERIFY
            books.Count.ShouldEqual(3);
        }

        [Fact]
        public void TestExampleSqlServerEnsureCleanAgain()
        { 
            TestExampleSqlServerEnsureClean();
        }

        [Fact]
        public void TestExampleSqlServerEnsureCleanAgainAgain()
        {
            TestExampleSqlServerEnsureClean();
        }
    }
}