﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using GenericEventRunner.DomainParts;

namespace BookApp.Books.Domain.DomainEvents
{
    public class BookReviewAddedEvent : IEntityEvent
    {
        public BookReviewAddedEvent(int numStars, Action<int,double> updateReviewCachedValues)
        {
            NumStars = numStars;

            UpdateReviewCachedValues = updateReviewCachedValues;
        }

        public int NumStars { get; }

        public Action<int, double> UpdateReviewCachedValues { get; }
    }
}