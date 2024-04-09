using FunWithAPIs;
using FunWithAPIs.Languages;
using FunWithAPIs.Translations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IApiConfigurer, LanguagesConfigurer>();
builder.Services.AddSingleton<IApiConfigurer, TranslationsConfigurer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Get the services
var apiConfigurers = app.Services.GetServices<IApiConfigurer>();
// Add the endpoints
foreach (var configurer in apiConfigurers)
{
    configurer.AddEndpoints(app);
}

app.Run();
