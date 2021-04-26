﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Test.Chapter12Listings.EventInterfacesEtc
{
    public interface IEntityEvents
    {
        void AddEvent(IDomainEvent domainEvent);
        ICollection<IDomainEvent> GetEventsThenClear();
    }
}