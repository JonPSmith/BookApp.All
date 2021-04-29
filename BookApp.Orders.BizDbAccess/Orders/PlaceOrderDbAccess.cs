// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.Orders.Domain;
using BookApp.Orders.Persistence.EfCoreSql;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Orders.BizDbAccess.Orders
{
    public class PlaceOrderDbAccess : IPlaceOrderDbAccess
    {
        private readonly OrderDbContext _context;

        public PlaceOrderDbAccess(OrderDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This finds any books that fits the BookIds given to it, with any optional promotion
        /// </summary>
        /// <param name="bookIds"></param>
        /// <returns>A dictionary with the BookId as the key, and the Book as the value</returns>
        public async Task<IDictionary<int, BookView>> FindBooksByIdsAsync(IEnumerable<int> bookIds)               
        {
            return await _context.BookViews                       
                .Where(x => bookIds.Contains(x.BookId)) 
                .ToDictionaryAsync(key => key.BookId);       
        }

        public Task AddAndSave(Order newOrder)                 
        {                                               
            _context.Orders.Add(newOrder);
            return _context.SaveChangesAsync();
        }
    }

}