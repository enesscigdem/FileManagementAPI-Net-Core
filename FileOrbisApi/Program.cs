﻿using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.BusinessLayer.Concrete;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<FileOrbisContext>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5174") // İzin verilen alan adı
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();

    });
});

builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericManager<>));
builder.Services.AddScoped(typeof(IGenericDal<>), typeof(GenericRepository<>));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSwagger();
app.UseAuthorization();

app.MapControllers();

app.Run();
