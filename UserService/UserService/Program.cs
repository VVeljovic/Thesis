using Microsoft.EntityFrameworkCore;
using UserService.DbContext;
using UserService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var Configuration = builder.Configuration;
builder.Services.AddDbContext<DBContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("Connection")));

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<DBContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    app.MapIdentityApi<User>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
