﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Test.Chapter12Listings.BusinessLogic;
using Test.Chapter12Listings.EventInterfacesEtc;
using Test.Chapter12Listings.Events;

namespace Test.Chapter12Listings.EventHandlers
{
    public class QuoteLocationChangedEventHandler : IEventHandler<QuoteLocationChangedEvent>
    {
        private readonly ICalcSalesTaxService _taxLookupService;

        public QuoteLocationChangedEventHandler(ICalcSalesTaxService taxLookupService)
        {
            _taxLookupService = taxLookupService;
        }

        public void HandleEvent(QuoteLocationChangedEvent domainEvent)
        {
            var salesTaxPercent = _taxLookupService.GetSalesTax(domainEvent.NewLocation?.State);
            domainEvent.QuoteToUpdate.SalesTaxPercent = salesTaxPercent;
        }
    }
}