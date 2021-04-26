// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;

namespace BookApp.Orders.Persistence.EfCoreSql
{
    public class ReplacementUserIdService : IUserIdService
    {
        public Guid GetUserId()
        {
            return Guid.Empty;
        }
    }
}