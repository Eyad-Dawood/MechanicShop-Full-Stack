
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Configuration.GetSection("AppSettings").Get<AppSettings>()!);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.AddServiceDefaults();


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "MechanicShop API V1");

        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.EnableFilter();
    });

    await app.InitialiseDatabaseAsync();

    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}

app.MapControllers();

app.MapDefaultEndpoints();

app.UseForwardedHeaders();

app.UseCoreMiddlewares(builder.Configuration);


app.Run();