using game.Server.Data;
using game.Server.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqliteOptions => {
        }
    )
);


builder.Services.AddScoped<MineGenerationService>();
builder.Services.AddScoped<ISaveService, SaveService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICombatService, CombatService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IDungeonService, DungeonService>();
builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IBlueprintService, BlueprintService>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped<IMineInteractionService, MineInteractionService>();
builder.Services.AddSingleton<MapGeneratorService>();
builder.Services.AddSingleton<CrypticWizard.RandomWordGenerator.WordGenerator>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend",
        policy => {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseForwardedHeaders();

app.UseDefaultFiles();
app.MapStaticAssets();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseRouting(); 

app.UseCors("AllowFrontend");


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();