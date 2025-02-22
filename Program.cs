using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PokesMan.Data;
using PokesMan.Repository;
using PokesMan.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Database Context
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Scoped Services
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<TokenService>();

// Add Controllers
builder.Services.AddControllers();

// Enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PokesMan API",
        Version = "v1"
    });
});

var app = builder.Build();

// Enable Swagger for all environments
app.UseSwagger();
app.UseSwaggerUI();

// Middleware
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
