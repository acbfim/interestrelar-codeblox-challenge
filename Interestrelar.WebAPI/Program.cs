using System.Text.Json.Serialization;
using Interestrelar.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<ILogger<DataContext>, Logger<DataContext>>();
builder.Services.AddSingleton<DataContext>();

builder.Services.AddMyDependencyGroup();
builder.Services.AddSwaggerConfig();
//builder.Services.AddAuth();

builder.Services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                });



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

app.UseCors("CorsPolicy");

ILoggerFactory loggerFactory = new LoggerFactory();
ILogger<DataContext> logger = loggerFactory.CreateLogger<DataContext>();
var cargo = new DataContext(logger);

Console.WriteLine($"YEAR: {DataContext._Cargo[0].Year}");

app.Run();


