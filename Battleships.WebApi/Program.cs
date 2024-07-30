using Battleships.Common.Settings;
using Battleships.Core.Services;
using Battleships.Services.Interfaces;
using Battleships.WebApi;

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
    options.Cookie.SameSite = SameSiteMode.None; // to allow cross-site requests
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:8080", "https://kalina559.github.io") // frontend URL
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();

        });
});

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
app.UseCors("AllowSpecificOrigin");
app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.Run();