



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);
//Add services to the container
builder.Services.AddBasketModule(builder.Configuration)
                .AddCatalogModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);
var app = builder.Build();

//Configure the HTTP request pipeline.
app.MapCarter();
app.UseCatalogModule()
                .UseBasketModule()
                .UseOrderingModule();

app.Run();
 