using AccommodationService.Application.Interfaces;
using AccommodationService.Infrastructure.InversionOfControl;
using AccommodationService.Infrastructure.MongoDb;
using AccommodationService.Infrastructure.Services;
using AccommodationService.Presentation.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddSingleton(mongoDbSettings);
builder.Services.AddInfrastructure();
builder.Services.AddHostedService<KafkaConsumerHosterService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
