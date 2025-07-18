

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
//Add services to the container
builder.Services.AddBasketModule(builder.Configuration)
                .AddCatalogModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();

//Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseSerilogRequestLogging();

app.UseCatalogModule()
                .UseBasketModule()
                .UseOrderingModule();


app.Run();
 