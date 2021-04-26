﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.Books.ServiceLayer.CachedSql;
using BookApp.Books.ServiceLayer.DisplayCommon;
using BookApp.Books.ServiceLayer.DisplayCommon.Dtos;
using BookApp.Books.ServiceLayer.GoodLinq;
using BookApp.Main.Infrastructure.LoggingServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApp.UI.Controllers
{
    public class CachedNoCountSqlController : BaseTraceController
    {
        public async Task<IActionResult> Index(SortFilterPageOptionsNoCount options, [FromServices] IListBooksCachedNoCountService service)
        {
            var bookList = await service.SortFilterPage(options)
                .ToListAsync();

            options.SetupRestOfDto(bookList.Count);
            SetupTraceInfo();

            return View(new BookListNoCountCombinedDto(options, bookList));
        }

        /// <summary>
        /// This provides the filter search dropdown content
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFilterSearchContent(SortFilterPageOptionsNoCount options, [FromServices] IBookFilterDropdownService service)
        {
            var traceIdent = HttpContext.TraceIdentifier;
            return Json(
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                    traceIdent,
                    service.GetFilterDropDownValues(
                        options.FilterBy)));
        }
    }
}