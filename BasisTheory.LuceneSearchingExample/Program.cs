using BasisTheory.LuceneSearchingExample.entities;
using BasisTheory.LuceneSearchingExample.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<SocietyDbContext>(options =>
        options.UseInMemoryDatabase("Society"))
    .AddScoped<IPersonsService, PersonsService>()
    .AddControllers();

var app = builder.Build();

using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<SocietyDbContext>();
context.Database.EnsureCreated();

app.UseRouting().UseEndpoints(endpoints => endpoints.MapControllers());

app.MapGet("/", () => "Welcome to Lucene's query parsing into EF expressions tutorial!");

app.Run();