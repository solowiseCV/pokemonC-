using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PokesMan.Data;
using PokesMan.Repository;
using PokesMan.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<TokenService>();


builder.Services.AddControllers();


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


app.UseSwagger();
app.UseSwaggerUI();


app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
