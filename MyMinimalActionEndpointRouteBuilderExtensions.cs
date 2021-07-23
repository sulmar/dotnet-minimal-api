using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api
{
    public static class MyMinimalActionEndpointRouteBuilderExtensions
    {
        public static MinimalActionEndpointConventionBuilder MapPatch(this IEndpointRouteBuilder endpoints, string pattern, Delegate action)
        {
            return endpoints.MapMethods(pattern, new string[] { "PATCH" }, action);
        }
    }
}
