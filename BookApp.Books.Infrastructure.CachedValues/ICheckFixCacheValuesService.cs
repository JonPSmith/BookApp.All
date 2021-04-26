﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookApp.Books.Infrastructure.CachedValues
{
    public interface ICheckFixCacheValuesService
    {
        Task<List<string>> RunCheckAsync(DateTime fromThisDate, bool fixBadCacheValues, CancellationToken cancellationToken);
    }
}