using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace PrzepisyWebApplication.Middlewares
{
    public class LastVisitMiddleware
    {
        private readonly RequestDelegate _next;
        public const string CookieName = "LAST_VISIT"; // nazwa cookie

        public LastVisitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Sprawdzamy, czy klient ma już ciasteczko LAST_VISIT
            if (context.Request.Cookies.ContainsKey(CookieName))
            {
                var visitDateString = context.Request.Cookies[CookieName];
                if (DateTime.TryParse(visitDateString, out DateTime visitDate))
                {
                    context.Items["LastVisit"] = visitDate;
                }
            }
            else
            {
                context.Items["LastVisit"] = null;
            }

            // 2. Ustawiamy/zaktualizujemy ciasteczko z obecną datą i godziną
            var nowString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            context.Response.Cookies.Append(CookieName, nowString, new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1), // ciasteczko ważne przez rok
                HttpOnly = false
            });

            // 3. Przechodzimy do kolejnego elementu potoku
            await _next(context);
        }
    }
}