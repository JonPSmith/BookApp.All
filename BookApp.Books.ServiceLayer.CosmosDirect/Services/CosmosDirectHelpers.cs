﻿// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using BookApp.Books.Persistence.CosmosDb;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Books.ServiceLayer.CosmosDirect.Services
{
    public static class CosmosDirectHelpers
    {
        public static Container GetCosmosContainerFromDbContext(this CosmosDbContext context, string databaseName)
        {
            var cosmosClient = context.Database.GetCosmosClient();
            var database = cosmosClient.GetDatabase(databaseName);
            return database.GetContainer(nameof(CosmosDbContext));
        }
    }
}