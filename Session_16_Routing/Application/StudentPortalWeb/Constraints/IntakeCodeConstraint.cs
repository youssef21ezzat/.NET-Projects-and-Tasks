using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace StudentPortalWeb.Constraints
{
    public class IntakeCodeConstraint : IRouteConstraint
    {
        // It must not touch the database because routing runs very early for every single request, and hitting the database here would severely degrade the app's performance.
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(routeKey, out var value) || value == null)
            {
                return false;
            }

            return string.Equals(value.ToString(), "itiB", StringComparison.OrdinalIgnoreCase);
        }
    }
}
