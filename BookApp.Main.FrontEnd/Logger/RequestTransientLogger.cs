﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using BookApp.Main.Infrastructure.LoggingServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace BookApp.UI.Logger
{
    /// <summary>
    /// This logger only logs for the current request, i.e. it overwrites the log when a new request starts
    /// </summary>
    public class RequestTransientLogger : ILoggerProvider
    {
        public const string NonHttpLogsIdentifier = "non-http-logs";

        private readonly Func<IHttpContextAccessor> _httpAccessor;

        public RequestTransientLogger(Func<IHttpContextAccessor> httpAccessor)
        {
            _httpAccessor = httpAccessor;
        }

        public static LogLevel LogThisAndAbove { get; set; } = LogLevel.Information;

        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger(_httpAccessor);
        }

        public void Dispose()
        {
        }

        private class MyLogger : ILogger
        {
            private readonly Func<IHttpContextAccessor> _httpAccessor;

            public MyLogger(Func<IHttpContextAccessor> httpAccessor)
            {
                _httpAccessor = httpAccessor;
                
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= LogThisAndAbove;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter)
            {
                var currHttpContext = _httpAccessor().HttpContext;
                HttpRequestLog.AddLog(currHttpContext?.TraceIdentifier ?? NonHttpLogsIdentifier,
                    logLevel, eventId, formatter(state, exception));
                if (currHttpContext != null && eventId.Name == "RequestFinished")
                {
                    var currentUrl = currHttpContext?.Request.GetEncodedUrl();
                    HttpTimingLog.AddLog(currentUrl, state.ToString());
                }
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}