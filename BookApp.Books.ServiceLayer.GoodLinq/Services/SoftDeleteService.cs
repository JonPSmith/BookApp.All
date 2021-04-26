// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using BookApp.Books.Persistence.EfCoreSql;

namespace BookApp.Books.ServiceLayer.GoodLinq.Services
{
    public class SoftDeleteService
    {
        private BookDbContext _context;

        public SoftDeleteService(BookDbContext context)
        {
            _context = context;
        }
    }
}