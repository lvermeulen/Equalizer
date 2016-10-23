using System;
using Equalizer.Middleware.Core;
using Owin;

namespace Equalizer.Middleware.Owin
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseEqualizer(this IAppBuilder app, EqualizerMiddlewareOptions middlewareOptions)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (middlewareOptions == null)
            {
                throw new ArgumentNullException(nameof(middlewareOptions));
            }

            app.Use<EqualizerMiddleware>(middlewareOptions);

            return app;
        }
    }
}
