using AccommodationService.Application.Dtos.ChoreographyDtos;
using AccommodationService.Application.Interfaces;
using AccommodationService.Infrastructure.InversionOfControl;
using AccommodationService.Infrastructure.MongoDb;
using AccommodationService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options=>{
            options.AddPolicy("AllowSpecificOrigins",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddSingleton(mongoDbSettings);
builder.Services.AddInfrastructure();
builder.Services.AddHostedService<RabbitMQConsumerHostedService<TransactionRequestDto>>();
builder.Services.AddHostedService<RabbitMQConsumerHostedService<TransactionResponseDto>>();
var app = builder.Build();
app.UseCors("AllowSpecificOrigins");
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
