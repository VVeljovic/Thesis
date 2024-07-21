using Keycloak.AuthServices.Authentication;
using KeyCloak.AuthService.Authentication;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddKeycloakAuthentication(builder.Configuration);
services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/", (ClaimsPrincipal user) => { app.Logger.LogInformation(user.Identity.Name); }).RequireAuthorization();
app.UseHttpsRedirection();



app.Run();

