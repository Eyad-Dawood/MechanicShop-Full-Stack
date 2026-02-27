var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.MechanicShop_Api>("mechanicshop-api")
                 .WithEnvironment("ASPNETCORE_FORWARDEDHEADERS_ENABLED", "true");

builder.Build().Run();
