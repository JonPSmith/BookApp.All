﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Test.Chapter12Listings.EventInterfacesEtc;
using Test.Chapter12Listings.Events;

namespace Test.Chapter12Listings.DomainEventEfClasses
{
    public class Location : AddEventsToEntity            //#A
    {
        private string _state;                           //#C
        public int LocationId { get; set; }              //#B
        public string Name { get; set; }                 //#B

        [MaxLength(20)]
        public string State                              //#D
        {
            get => _state;
            set                                          //#E
            {                                            //#E
                if (value != _state)                     //#E
                    AddEvent(                            //#E
                        new LocationChangedEvent(this)); //#E
                _state = value;                          //#E
            }                                            //#E
        }

        public override string ToString()
        {
            return $"Name: {Name}, State: {State}";
        }
    }
    /***********************************************************
    #A This entity class inherits the AddEventsToEntity to gain the ability to use events
    #B These are normal properties that don't generate events when they are changed
    #C The backing field contains the real value of the data
    #D The setter is changed to sent a LocationChangedEvent if the State value changes 
    #E This code will add a LocationChangedEvent to the entity class if the State value changed
     **************************************************************/

}