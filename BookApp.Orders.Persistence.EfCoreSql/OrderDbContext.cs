﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Reflection;
using BookApp.Common.Persistence;
using BookApp.Orders.Domain;
using BookApp.Orders.Domain.SupportTypes;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Orders.Persistence.EfCoreSql
{
    public class OrderDbContext : DbContext, IUserId                   
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options,   
            IUserIdService userIdService = null)                        
            : base(options)                                           
        {                                                             
            UserId = userIdService?.GetUserId()                         
                     ?? new ReplacementUserIdService().GetUserId();     
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<BookView> BookViews { get; set; }
        public Guid UserId { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)                 
        {
            modelBuilder.AutoConfigureTypes();
            modelBuilder.AutoConfigureQueryFilters<OrderDbContext>(this);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
/******************************************************************************
* NOTES ON MIGRATION:
*
* BookApp.Frontend has two application DbContexts, BookDbContext and OrderDbContest
* Each has its own project, migrations and migration history table
* You need to build a migration from the DbContext's project (see below)
*
* NOTE: The EF Core commands give a error, but it does create the migration
* 
* see https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/migrations?tabs=visual-studio
* 
* The following NuGet libraries must be loaded
* 1. Add to BookApp: "Microsoft.EntityFrameworkCore.Tools"
* 2. Add to DataLayer: "Microsoft.EntityFrameworkCore.SqlServer" (or another database provider)
* 
* 2. Using Package Manager Console commands
* The steps are:
* a) Make sure the default project is BookApp.Orders.Persistence.EfCoreSql
* b) Use the PMC command
*    Add-Migration NameForMigration -Context OrderDbContext
* c) Use PMC command
*    Update-database (or migrate on startup)
*    
* If you want to start afresh then:
* a) Delete the current database
* b) Delete all the class in the Migration directory
* c) follow the steps to add a migration
******************************************************************************/