﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.Books.Persistence.CosmosDb;
using BookApp.Books.ServiceLayer.CosmosDirect;
using BookApp.Books.ServiceLayer.CosmosDirect.Services;
using BookApp.Books.ServiceLayer.DisplayCommon;
using BookApp.Books.ServiceLayer.DisplayCommon.Dtos;
using BookApp.Main.Infrastructure;
using BookApp.Main.Infrastructure.LoggingServices;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.UI.Controllers
{
    public class CosmosDirectController : BaseTraceController
    {
        public async Task<IActionResult> Index(SortFilterPageOptions options, 
            [FromServices] CosmosDbContext context,
            [FromServices] BookAppSettings settings)
        {
            options.SetupRestOfDto(await context.CosmosDirectCountAsync(options, settings.CosmosDatabaseName));
            var bookList = (await context.CosmosDirectQueryAsync(options, settings.CosmosDatabaseName)).ToList();

            SetupTraceInfo();

            return View(new CosmosDirectBookListCombinedDto(options, bookList));
        }

        /// <summary>
        /// This provides the filter search dropdown content
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetFilterSearchContent(SortFilterPageOptions options,
            [FromServices] CosmosDbContext context,
            [FromServices] BookAppSettings settings)
        {
            var traceIdent = HttpContext.TraceIdentifier;
            var dropdowns = await context.GetFilterDropDownValuesAsync(options.FilterBy, settings.CosmosDatabaseName);
            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                    traceIdent, dropdowns));
        }
    }
}