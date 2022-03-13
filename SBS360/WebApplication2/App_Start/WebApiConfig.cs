using Eng360Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Eng360Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //config.Routes.MapHttpRoute(
            //         name: "ActionApi_V1",
            //         routeTemplate: "api/v1/{controller}/{action}/{id}",
            //         defaults: new { id = RouteParameter.Optional }
            //     );

            //Global error handling in WebAPI through the ExceptionFilterAttribute
            //Refer link : https://blogs.msdn.microsoft.com/webdev/2012/11/16/capturing-unhandled-exceptions-in-asp-net-web-apis-with-elmah/
            config.Filters.Add(new UnhandledExceptionFilter());
        }
    }
}
