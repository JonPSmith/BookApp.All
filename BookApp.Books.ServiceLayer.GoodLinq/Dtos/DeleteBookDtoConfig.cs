﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using AutoMapper;
using BookApp.Books.Domain;
using GenericServices.Configuration;

namespace BookApp.Books.ServiceLayer.GoodLinq.Dtos
{
    class DeleteBookDtoConfig : PerDtoConfig<DeleteBookDto, Book>
    {
        public override Action<IMappingExpression<Book, DeleteBookDto>> AlterReadMapping
        {
            get
            {
                return cfg => cfg
                    .ForMember(x => x.AuthorsOrdered, y => 
                        y.MapFrom(p => string.Join(", ",
                        p.AuthorsLink.OrderBy(q => q.Order).Select(q => q.Author.Name).ToList())));
            }
        }
    }
}