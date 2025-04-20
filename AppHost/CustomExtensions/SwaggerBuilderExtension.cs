using System.Diagnostics;

namespace AppHost.CustomExtensions;

internal static class SwaggerBuilderExtension
{
    public static IResourceBuilder<T> WithSwaggerUi<T>(this IResourceBuilder<T> builder)
        where T : IResourceWithEndpoints
    {
        return builder.WithCommand(name: "swagger-ui-docs", displayName: "Swagger API Documentation",
            executeCommand: _ =>
            {
                try
                {
                    EndpointReference endpoint = builder.GetEndpoint("https");
                    string url = $"{endpoint.Url}/swagger";
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                    return Task.FromResult(new ExecuteCommandResult() { Success = true });
                }
                catch (ArgumentNullException e)
                {
                    return Task.FromResult(new ExecuteCommandResult() { Success = false, ErrorMessage = e.Message });
                }
            },
            commandOptions: new CommandOptions()
            {
                IconName = "LinkMultiple", IsHighlighted = true, IconVariant = IconVariant.Regular
            }
        );
    }
}
