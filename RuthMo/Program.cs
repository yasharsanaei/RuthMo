using Microsoft.EntityFrameworkCore;
using RuthMo.Models;
using static Microsoft.AspNetCore.Builder.WebApplication;

var builder = CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RuthMoPostgres");
var corsPolicyName = "RuthMoCors";


builder.Services.AddDbContext<MotivationContext>(options => { options.UseNpgsql(connectionString); });

builder.Logging.AddConsole();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicyName);

app.MapControllers();
app.Run();