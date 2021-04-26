// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.Books.Persistence.EfCoreSql;
using BookApp.Books.ServiceLayer.DapperSql.DapperCode;
using BookApp.Books.ServiceLayer.DisplayCommon;
using BookApp.Books.ServiceLayer.DisplayCommon.Dtos;
using BookApp.Books.ServiceLayer.GoodLinq;
using BookApp.Infrastructure.LoggingServices;
using BookApp.Persistence.EfCoreSql.Books;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.UI.Controllers
{
    public class DapperSqlController : BaseTraceController
    {
        public async Task<IActionResult> Index(SortFilterPageOptions options, [FromServices] BookDbContext context)
        {
            options.SetupRestOfDto(await context.DapperBookListCountAsync(options));
            var bookList = (await context.DapperBookListQueryAsync(options)).ToList();

            SetupTraceInfo();

            return View(new BookListCombinedDto(options, bookList));
        }

        /// <summary>
        /// This provides the filter search dropdown content
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFilterSearchContent(SortFilterPageOptions options, [FromServices] IBookFilterDropdownService service)
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