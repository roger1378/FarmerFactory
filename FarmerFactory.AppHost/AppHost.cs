var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.FarmerFactory_Api>("api");

builder.AddProject<Projects.FarmerFactory_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
