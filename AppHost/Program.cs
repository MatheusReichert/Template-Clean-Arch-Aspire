using AppHost.CustomExtensions;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Web_API>("API").WithSwaggerUi();

await builder.Build().RunAsync().ConfigureAwait(false);
