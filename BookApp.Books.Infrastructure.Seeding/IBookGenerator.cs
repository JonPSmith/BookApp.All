﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookApp.Books.Infrastructure.Seeding
{
    public interface IBookGenerator
    {
        Task<TimeSpan> WriteBooksAsync(string wwwRootDir, bool wipeDatabase, int totalBooksNeeded, bool makeBookTitlesDistinct, CancellationToken cancellationToken);
    }
}