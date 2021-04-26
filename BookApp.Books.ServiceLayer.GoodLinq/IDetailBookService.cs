// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using BookApp.Books.ServiceLayer.GoodLinq.Dtos;

namespace BookApp.Books.ServiceLayer.GoodLinq
{
    public interface IDetailBookService
    {
        Task<BookDetailDto> GetBookDetailAsync(int bookId);
    }
}