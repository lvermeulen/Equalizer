using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Equalizer.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEqualizer(this IApplicationBuilder app, EqualizerMiddlewareOptions middlewareOptions)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (middlewareOptions == null)
            {
                throw new ArgumentNullException(nameof(middlewareOptions));
            }

            return app.UseMiddleware<EqualizerMiddleware>(Options.Create(middlewareOptions));
        }
    }
}
