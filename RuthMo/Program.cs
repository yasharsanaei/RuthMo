var builder = WebApplication.CreateBuilder(args);

/*
   using MotivationContext context = new MotivationContext();

   Author author = new Author()
   {
       Name = "Yashar",
       NickName = "Batman"
   };

   context.Add(author);
   context.SaveChanges();
 */

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: AllowAnyOrigin must be fixed!
builder.Services.AddCors(options => { options.AddDefaultPolicy(policy => policy.AllowAnyOrigin()); });

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

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();