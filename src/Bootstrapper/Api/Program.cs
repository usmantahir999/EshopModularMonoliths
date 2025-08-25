
using Keycloak.AuthServices.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
//Add carter for all modules
//common services: carter, mediatr, fluentvalidation, masstransit
var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;
var orderingAssembly = typeof(OrderingModule).Assembly;
builder.Services.AddCarterWithAssemblies(catalogAssembly, basketAssembly);
//Add mediatR, FluentValidation, and logging behaviors for all modules
builder.Services.AddMediatRWithAssemblies(catalogAssembly, basketAssembly);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddMassTransitExtensions(builder.Configuration, catalogAssembly, basketAssembly);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

//Add services to the container
builder.Services.AddBasketModule(builder.Configuration)
                .AddCatalogModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();

//Configure the HTTP request pipeline.
app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });
app.UseAuthentication();
app.UseAuthorization();
app.UseCatalogModule()
                .UseBasketModule()
                .UseOrderingModule();


app.Run();
 