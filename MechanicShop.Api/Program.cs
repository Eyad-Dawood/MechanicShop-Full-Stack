using MechanicShop.Api.Initialisers;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();
await app.InitialiseDatabaseAsync();



app.Run();
