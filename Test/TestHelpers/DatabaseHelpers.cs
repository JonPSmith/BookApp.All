// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Test.TestHelpers
{
    public static class DatabaseHelpers
    {
        public static string[] GetTableNamesInSqliteDb(this DbContext context)
        {
            var connection = context.Database.GetDbConnection();
            return connection.Query<string>($"SELECT name FROM {connection.Database}.sqlite_master WHERE type='table'").ToArray();
        }
    }
}