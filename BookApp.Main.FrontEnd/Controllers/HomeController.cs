// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using BookApp.Books.Persistence.EfCoreSql;
using BookApp.Main.FrontEnd.HelperExtensions;
using BookApp.Main.FrontEnd.Models;
using BookApp.Main.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Main.FrontEnd.Controllers
{
    public class HomeController : BaseTraceController
    {
        public IActionResult Index([FromServices]BookAppSettings settings)
        {
            return View(null, settings.Title);
        }

        public IActionResult DatabaseCounts([FromServices] BookDbContext context)
        {
            return View((object) new DatabaseStatsDto(context));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Chapter15Setup()
        {
            return View();
        }

        public IActionResult Chapter16Setup()
        {
            return View();
        }

        public IActionResult About()
        {
            var isLocal = Request.IsLocal();
            return View((object) isLocal);
        }

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        public IActionResult Error()
        {
            return View((object) new ErrorViewModel
                { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}