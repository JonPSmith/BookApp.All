// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApp.Books.Infrastructure.CosmosDb
{
    public interface IBookToCosmosBookService
    {
        Task AddCosmosBookAsync(int bookId);
        Task UpdateCosmosBookAsync(int bookId);
        Task DeleteCosmosBookAsync(int bookId);
        Task UpdateManyCosmosBookAsync(List<int> bookIds);
    }
}