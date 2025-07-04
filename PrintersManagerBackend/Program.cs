using Microsoft.EntityFrameworkCore;
using PrintersManagerBackend.Common;
using PrintersManagerBackend.Context;
using PrintersManagerBackend.Interfaces;
using PrintersManagerBackend.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(ConnectionConfig.ConnectionString);
});

builder.Services.AddScoped<IPrinterService, PrinterService>();
builder.Services.AddHostedService<PrinterPollingService>();

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
