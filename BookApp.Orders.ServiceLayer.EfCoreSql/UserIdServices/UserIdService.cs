// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using BookApp.Orders.BizLogic.BasketServices;
using BookApp.Orders.Persistence.EfCoreSql;
using BookApp.Orders.ServiceLayer.EfCoreSql.CheckoutServices.Concrete;
using Microsoft.AspNetCore.Http;

namespace BookApp.Orders.ServiceLayer.EfCoreSql.UserIdServices
{
    public class UserIdService : IUserIdService
    {
        private readonly IHttpContextAccessor _httpAccessor;

        public UserIdService(IHttpContextAccessor httpAccessor)         
        {                                                               
            _httpAccessor = httpAccessor;                               
        }

        public Guid GetUserId()
        {
            var httpContext = _httpAccessor.HttpContext;                
            if (httpContext == null)                                    
                return Guid.Empty;                                      

            var cookie = new BasketCookie(httpContext.Request.Cookies); 
            if (!cookie.Exists())                                       
                return Guid.Empty;                                      

            var service = new CheckoutCookieService(cookie.GetValue()); 
            return service.UserId;                                      
        }
    }
}