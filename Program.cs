using Microsoft.EntityFrameworkCore;
using PoemGenerator.Monolith.Data;
using PoemGenerator.Monolith.Repositories;
using PoemGenerator.Monolith.Services;
using MediatR;
using System.Reflection;
using PoemGenerator.Monolith.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<INotificationService, RabbitMqNotificationService>();


// Configure Entity Framework with Postgres
string? constr = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(constr))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
}
builder.Services.AddDbContext<PoemDbContext>(options =>
    options.UseNpgsql(constr));

// Register repository and services
builder.Services.AddScoped<IPoemRepository, PoemRepository>();
builder.Services.AddScoped<IPoemService, PoemService>();

var app = builder.Build();

// Apply database migrations on startup
/* using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PoemDbContext>();
    dbContext.Database.Migrate();
} */

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();