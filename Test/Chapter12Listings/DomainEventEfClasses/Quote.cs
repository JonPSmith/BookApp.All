﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Test.Chapter12Listings.EventInterfacesEtc;
using Test.Chapter12Listings.Events;

namespace Test.Chapter12Listings.DomainEventEfClasses
{
    public class Quote : AddEventsToEntity
    {
        private Location _whereInstall;
        private Quote(){} //needed by EF Core

        public Quote(Location location)
        {
            if (location == null && _whereInstall == null)
                //This ensures a new Quote creates an location event
                AddEvent(new QuoteLocationChangedEvent(this,null));
            WhereInstall = location;
        }


        public int QuoteId { get; set; }

        public double Price { get; set; }

        public double PriceWithSalesTax { get; set; }

        public double SalesTaxPercent { get; set; }

        //----------------------------------------------
        //relationships

        public int? LocationId { get; set; }

        public Location WhereInstall
        {
            get => _whereInstall;
            set
            {
                if (value != _whereInstall)
                    AddEvent(new QuoteLocationChangedEvent(this, value));
                _whereInstall = value;
            }
        }
    }
}