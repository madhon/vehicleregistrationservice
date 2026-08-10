namespace VehicleRegistrationService.Extensions;

// Extensions/SecurityHeadersExtensions.cs
internal static class SecurityHeadersExtensions
{
    // Key used by Scalar.AspNetCore when WithNonce() is enabled — confirm against your package version.
    // Docs: value is in HttpContext.Items for CSP script-src 'nonce-...'
    private const string ScalarNonceItemKey = "ScalarNonce"; // verify in Scalar source/docs for your version

    public static IApplicationBuilder UseAppSecurityHeaders(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            context.Response.OnStarting(() =>
            {
                var headers = context.Response.Headers;
                var path = context.Request.Path;

                headers.XContentTypeOptions = "nosniff";
                headers.XFrameOptions = "DENY";
                headers["Referrer-Policy"] = "no-referrer";
                headers["Permissions-Policy"] = "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()";
                headers["Cross-Origin-Opener-Policy"] = "same-origin";
                headers["Cross-Origin-Resource-Policy"] = "same-origin";

                // HSTS only over HTTPS (no-op on plain http://localhost:57123)
                if (context.Request.IsHttps)
                {
                    headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
                }

                var isScalarOrOpenApi =
                    path.StartsWithSegments("/scalar", StringComparison.OrdinalIgnoreCase)
                    || path.StartsWithSegments("/openapi", StringComparison.OrdinalIgnoreCase);

                if (isScalarOrOpenApi)
                {
                    // Read nonce Scalar put in Items (name may be "scalar-nonce" / package-specific — check Items after WithNonce())
                    var nonce = context.Items[ScalarNonceItemKey] as string;
                    var scriptSrc = string.IsNullOrEmpty(nonce)
                        ? "script-src 'self' 'unsafe-inline'"           // fallback if nonce missing
                        : $"script-src 'self' 'nonce-{nonce}'";

                    // style-src 'unsafe-inline' is required by Scalar today (inline style attributes)
                    headers.ContentSecurityPolicy =
                        "default-src 'self'; " +
                        $"{scriptSrc}; " +
                        "style-src 'self' 'unsafe-inline'; " +
                        "img-src 'self' data: blob:; " +
                        "font-src 'self'; " +
                        "connect-src 'self'; " +          // OpenAPI doc + try-it-out API calls
                        "frame-ancestors 'none'; " +
                        "base-uri 'self'; " +
                        "form-action 'self'";
                }
                else
                {
                    // API JSON responses: lock CSP down; browsers rarely execute API bodies anyway
                    headers.ContentSecurityPolicy = "default-src 'none'; frame-ancestors 'none'; base-uri 'none'";
                    headers.CacheControl = "no-store";
                }

                return Task.CompletedTask;
            });

            await next();
        });
    }
}
