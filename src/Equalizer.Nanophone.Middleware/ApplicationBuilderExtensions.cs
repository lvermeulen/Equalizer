using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Equalizer.Nanophone.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEqualizer(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<EqualizerNanophoneMiddleware>();
        }

        public static IApplicationBuilder UseEqualizer(this IApplicationBuilder app, EqualizerOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<EqualizerNanophoneMiddleware>(Options.Create(options));
        }
    }
}
