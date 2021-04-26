// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using BookApp.Books.Domain;
using BookApp.Books.Domain.DomainEvents;
using BookApp.Books.Persistence.EfCoreSql;
using GenericEventRunner.ForHandlers;
using Microsoft.EntityFrameworkCore;
using NetCore.AutoRegisterDi;
using StatusGeneric;

namespace BookApp.Books.Infrastructure.CosmosDb.EventsHandlers
{
    [DoNotAutoRegister]
    public class AuthorNameUpdatedHandlerAsync : IDuringSaveEventHandlerAsync<AuthorNameUpdatedEvent>
    {
        private readonly IBookToCosmosBookService _service;
        private readonly BookDbContext _sqlContext;

        public AuthorNameUpdatedHandlerAsync(BookDbContext sqlContext, IBookToCosmosBookService service)
        {
            _sqlContext = sqlContext;
            _service = service;
        }

        public async Task<IStatusGeneric> HandleAsync(object callingEntity, AuthorNameUpdatedEvent domainEvent, Guid uniqueKey)
        {

            var bookIds = await _sqlContext.Authors
                .Where(x => x.AuthorId == ((Author) callingEntity).AuthorId)
                .SelectMany(x => x.BooksLink.Select(y => y.BookId)).ToListAsync();

            await _service.UpdateManyCosmosBookAsync(bookIds);
            return null;
        }
    }
}