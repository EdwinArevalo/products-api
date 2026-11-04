using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ProductosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Productos")));

builder.Services.AddCors(options =>
    options.AddPolicy("Front", policy => policy
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

// Se deja habilitado también en la nube para poder probar la API publicada
app.MapOpenApi();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));

app.UseHttpsRedirection();
app.UseCors("Front");
app.UseAuthorization();
app.MapControllers();

app.Run();
