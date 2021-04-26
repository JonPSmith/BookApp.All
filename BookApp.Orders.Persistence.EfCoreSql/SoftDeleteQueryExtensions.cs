﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using System.Reflection;
using BookApp.Orders.Domain.SupportTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookApp.Orders.Persistence.EfCoreSql
{
    public enum MyQueryFilterTypes { SoftDelete, UserId }       

    public static class SoftDeleteQueryExtensions              
    {
        public static void AddSoftDeleteQueryFilter(            
            this IMutableEntityType entityData,                 
            MyQueryFilterTypes queryFilterType,                 
            IUserId userIdProvider = null)                      
        {
            var methodName = $"Get{queryFilterType}Filter";       
            var methodToCall = typeof(SoftDeleteQueryExtensions)  
                .GetMethod(methodName,                            
                    BindingFlags.NonPublic | BindingFlags.Static) 
                .MakeGenericMethod(entityData.ClrType);           
            var filter = methodToCall                             
                .Invoke(null, new object[] { userIdProvider });   
            entityData.SetQueryFilter((LambdaExpression)filter);   
        }

        private static LambdaExpression GetUserIdFilter<TEntity>(     
            IUserId userIdProvider)                                   
            where TEntity : class, IUserId                            
        {                                                             
            Expression<Func<TEntity, bool>> filter =                  
                x => x.UserId == userIdProvider.UserId;               
            return filter;                                            
        }
    }
}