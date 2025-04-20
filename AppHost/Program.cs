// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AppHost.CustomExtensions;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Web_API>("API").WithSwaggerUi();

await builder.Build().RunAsync().ConfigureAwait(false);
