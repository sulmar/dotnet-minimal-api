using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using minimal_api;
using minimal_api.FakeRepositories;
using minimal_api.Fakers;
using minimal_api.IRepositories;
using minimal_api.Models;
using System;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Faker<Customer>, CustomerFaker>();
builder.Services.AddSingleton<ICustomerRepository, FakeCustomerRepository>();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Description = "Docs for my API", Version = "v1" });
});

var app = builder.Build();
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => "Hello World!");

// app.MapGet("/api/pi", () => Math.PI);

app.MapGet("/api/pi", () => new { Result = Math.PI });

//app.MapGet("/customers/{id}", (int id, ICustomerRepository customerRepository) =>
//{
//    var customer = customerRepository.Get(id);

//    if (customer == null)
//        return Results.NotFound();

//    return Results.Ok(customer);
//}).WithName("GetCustomerById")
//.Produces<Customer>(StatusCodes.Status200OK)
//.Produces(StatusCodes.Status404NotFound);


// Z u¿yciem operatora is
app.MapGet("/customers/{id}", (int id, ICustomerRepository repository) =>
    repository.Get(id)
        is Customer customer
        ? Results.Ok(customer)
        : Results.NotFound())
    .Produces<Customer>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

// Z u¿yciem operatora switch
//app.MapGet("/customers/{id}", (int id, ICustomerRepository repository) => repository.Get(id) switch
//    {
//        Customer customer => Results.Ok(customer),
//        null => Results.NotFound()        
//    })
//    .Produces<Customer>(StatusCodes.Status200OK)
//    .Produces(StatusCodes.Status404NotFound);

app.MapGet("/customers", (ICustomerRepository customerRepository) => customerRepository.Get());

app.MapPost("/customers", (ICustomerRepository customerRepository, Customer customer) =>
{
    customerRepository.Add(customer);

    return Results.CreatedAtRoute("GetCustomerById", new { Id = customer.Id }, customer);
}).Produces<Customer>(StatusCodes.Status201Created);

app.MapDelete("/customers/{id}", (ICustomerRepository customerRepository, int id) =>
{
    if (customerRepository.Get(id) is Customer customer)
    {
        customerRepository.Remove(id);
        return Results.Ok();
    }
    else
    {
        return Results.NotFound();
    }
})
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);


app.MapPut("/customers", (ICustomerRepository customerRepository, int id, Customer customer) =>
{
    if (id != customer.Id)
    {
        return Results.BadRequest();
    }

    if (!customerRepository.IsExists(id))
    {
        return Results.NotFound();
    }

    customerRepository.Update(customer);

    return Results.Ok();

});

app.MapPatch("/customers", (ICustomerRepository customerRepository, Customer customer) => customerRepository.Update(customer));

app.MapPost("/api/token", (LoginViewModel model) =>
{

    var result = Results.Ok();

    return result;

});

app.MapGet("/api/items", (ICustomerRepository customerRepository, ClaimsPrincipal user) =>
{
    var tenantId = user.FindFirstValue("TenantId");
    return customerRepository.Get();
})
.RequireAuthorization();


app.Run();
