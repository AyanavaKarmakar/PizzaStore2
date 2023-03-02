using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PizzaDB>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "PizzaStore API",
        Description = "Making the pizzas you love",
        Version = "v2"

    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "PizzaStore API v2");
});

// routes
// get all pizzas
app.MapGet("/pizzas", async (PizzaDB db) => await db.Pizzas.ToListAsync());

// create a new pizza entry
app.MapPost("/pizza", async (PizzaDB db, Pizza pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();

    return Results.Created($"/pizza/{pizza.Id}", pizza);
});

// get a pizza by id
app.MapGet("/pizza/{id}", async (PizzaDB db, int id) => await db.Pizzas.FindAsync(id));

app.Run();
