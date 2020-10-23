﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routing
{
    public class RouterConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if(RouteDirection.IncomingRequest == routeDirection)
            {
                var v = values[routeKey];
                if(long.TryParse(v.ToString(), out long value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
