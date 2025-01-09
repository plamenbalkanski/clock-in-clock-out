using Microsoft.Extensions.DependencyInjection;
using TimeClockApi.Dal;
using TimeClockApi.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCsla();
builder.Services.AddTransient<ITimeClockDal, TimeClockDal>();
builder.Services.AddTransient<ITimeClockDataAccess, TimeClockDataAccess>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run(); 