using TransactionService.Application.Interfaces;
using TransactionService.Infrastructure.Services;
using TransactionService.Application.InversionOfControl;
using TransactionService.Infrastructure.InversionOfControl;
using TransactionService.Infrastructure.MongoDb;
using TransactionService.Application.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options=>{
            options.AddPolicy("AllowSpecificOrigins",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
});
builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddSingleton(mongoDbSettings);
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddHostedService<RabbitMQConsumerHostedService<TransactionResponseDto>>(); var app = builder.Build();app.UseCors("AllowSpecificOrigins");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<NotificationService>("notifications");
app.UseAuthorization();

app.MapControllers();

app.Run();
