using Battleships.Common.Settings;
using Battleships.Core.Services;
using Battleships.Services.Interfaces;
using Battleships.WebApi;
using System;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None; // Set SameSite to None for cross-site requests
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Always to ensure cookies are only sent over HTTPS

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:8080", "https://kalina559.github.io") // Your frontend URL
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials(); // Allow credentials

        });
});

// Register application services
builder.Services.AddScoped<IShipLocationService, ShipLocationService>();
builder.Services.AddScoped<IGenerateMoveService, GenerateMoveService>();
builder.Services.AddScoped<IGameStateService, GameStateService>();
builder.Services.AddScoped<IAiTypeService, AiTypeService>();
builder.Services.AddScoped<IRuleTypeService, RuleTypeService>();
builder.Services.AddScoped<ICosmosDbService, CosmosDbService>();

var cosmosDbSettings = new CosmosDbSettings();
builder.Configuration.Bind(nameof(CosmosDbSettings), cosmosDbSettings);
builder.Services.AddSingleton(cosmosDbSettings);

ServiceCollectionSetup.InitializeCosmosClientInstanceAsync(cosmosDbSettings, builder.Services);


// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigin"); // Use the specific CORS policy
app.UseSession(); // Ensure this is called before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
