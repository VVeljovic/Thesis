using Keycloak.AuthServices.Authentication;
using Microsoft.Extensions.Options;
using UserService.Infrastructure.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration, cfg => {
    cfg.Audience = "public-client";
    cfg.RequireHttpsMetadata = false;
});
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
