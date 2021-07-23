using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using minimal_api.FakeRepositories;
using minimal_api.Fakers;
using minimal_api.IRepositories;
using minimal_api.Models;

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

app.MapGet("/customers/{id}", (ICustomerRepository customerRepository, int id) =>
{
    var customer = customerRepository.Get(id);

    return new OkObjectResult(customer);
});

app.MapGet("/customers", (ICustomerRepository customerRepository) => customerRepository.Get());

app.MapPost("/customers", (ICustomerRepository customerRepository, Customer customer) => customerRepository.Add(customer));

app.MapDelete("/customers/{id}", (ICustomerRepository customerRepository, int id) => customerRepository.Remove(id));

    
app.Run();
