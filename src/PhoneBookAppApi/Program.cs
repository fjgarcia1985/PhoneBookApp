using Microsoft.EntityFrameworkCore;
using PhoneBookAppApi;
using PhoneBookAppApi.Models;
using System;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PhoneBookContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PhoneBookAPPDB")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/contacts", async (PhoneBookContext context) => Results.Ok(await context.Contacts.ToListAsync()));

app.MapGet("/contacts/{id}", async (int id, PhoneBookContext context) =>
{
    var contact = await context.Contacts.FindAsync(id);
    return contact != null ? Results.Ok(contact) : Results.NotFound();
});

app.MapPost("/contacts", async (ContactRequest contact, PhoneBookContext context) =>
{
    var createdContact = context.Contacts.Add(new Contact
    {
        FirstName = contact.FirstName ?? string.Empty,
        LastName = contact.LastName ?? string.Empty,
        PhoneNumber = contact.PhoneNumber ?? string.Empty,
    });
    await context.SaveChangesAsync();
    return Results.Created($"/contacts/{createdContact.Entity.ContactId}", createdContact.Entity);
});

app.MapDelete("/contacts/{id}", async (int id, PhoneBookContext context) =>
{
    var contact = await context.Contacts.FindAsync(id);
    if (contact == null)
    {
        return Results.NotFound();
    }
    context.Contacts.Remove(contact);
    await context.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/contacts/{id}", async (int id, ContactRequest contact, PhoneBookContext context) =>
{
    var contactToUpdate = await context.Contacts.FindAsync(id);

    if (contactToUpdate == null)
        return Results.NotFound();
    if (contact.FirstName != null)
        contactToUpdate.FirstName = contact.FirstName;
    if (contact.LastName != null)
        contactToUpdate.LastName = contact.LastName;
    if (contact.PhoneNumber != null)
        contactToUpdate.PhoneNumber = contact.PhoneNumber;
    await context.SaveChangesAsync();
    return Results.Ok(contactToUpdate);
});

app.Run();

internal record ContactRequest(string? FirstName, string? LastName, string? PhoneNumber);