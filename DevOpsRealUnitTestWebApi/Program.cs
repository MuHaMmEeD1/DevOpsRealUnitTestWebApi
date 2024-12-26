using DevOpsRealUnitTestWebApi.DataBase;
using DevOpsRealUnitTestWebApi.Repositorise;
using DevOpsRealUnitTestWebApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// DB Start

string connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<ProductDbContext>(options => {

    options.UseSqlServer(connectionString);

});

// DB End


// Repostorise Start

builder.Services.AddScoped<IProductRepository, ProductRepository>();
// Repostorise End


// Services Start

builder.Services.AddScoped<IProductService, ProductService>();
// Services End




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
