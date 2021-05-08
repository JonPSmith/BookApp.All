# BookApp.All

This is a version of the BookApp used in the [evolving modular monoliths series](https://www.thereformedprogrammer.net/evolving-modular-monoliths-1-an-architecture-for-net/). The BookApp is a e-commerce web app taht sells books using ASP.NET Core and EF Core. This application features in my book [Entity Framework Core in Action](https://bit.ly/EfCoreBook2).

This specific version:

- Uses the [modularize bounded context approach](https://www.thereformedprogrammer.net/evolving-modular-monoliths-1-an-architecture-for-net/#3-modularize-inside-a-bounded-context), where each project is focused on one job.
- Contains all of the projects to build the BookApp, i.e., it doesn't have any part of the code turned into a NuGet package (See part 2 - coming soon).


