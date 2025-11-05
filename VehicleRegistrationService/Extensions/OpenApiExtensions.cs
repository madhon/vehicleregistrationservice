namespace VehicleRegistrationService.Extensions;

using Asp.Versioning;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

internal static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new UrlSegmentApiVersionReader());
        });
        services.AddEndpointsApiExplorer();
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "Vehicle Registration API",
                    Version = "1.0.0",
                    Description = "Vehicle Registration API",
                    Contact = new OpenApiContact
                    {
                        Name = "Vehicle Registration API Team",
                    },
                };

                document.Servers = [];

                return Task.CompletedTask;
            });
            options.AddScalarTransformers();
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            options.CustomSchemaIds(x => x.FullName?.Replace("+", ".", StringComparison.OrdinalIgnoreCase));
        });

        return services;
    }

    private static OpenApiOptions CustomSchemaIds(this OpenApiOptions config,
        Func<Type, string?> typeSchemaTransformer,
        bool includeValueTypes = false)
    {
        return config.AddSchemaTransformer((schema, context, _) =>
        {
            // Skip value types and strings
            if (!includeValueTypes && 
                (context.JsonTypeInfo.Type.IsValueType || 
                 context.JsonTypeInfo.Type == typeof(String) || 
                 context.JsonTypeInfo.Type == typeof(string)))
            {
                return Task.CompletedTask;
            }

            // Skip if the schema ID is not already set because we don't want to decorate the schema multiple times
            if (schema.Annotations == null || !schema.Annotations.TryGetValue("x-schema-id", out var _))
            {
                return Task.CompletedTask;
            }

            // transform the typename based on the provided delegate
            var transformedTypeName = typeSchemaTransformer(context.JsonTypeInfo.Type);

            // Scalar - decorate the models section
            schema.Annotations["x-schema-id"] = transformedTypeName;

            // Swagger and Scalar specific:
            // for Scalar - decorate the endpoint section
            // for Swagger - decorate the endpoint and model sections
            schema.Title = transformedTypeName;

            return Task.CompletedTask;
        });
    }
}