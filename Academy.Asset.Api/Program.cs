using Academy.Asset.Api.Dtos;
using Academy.Asset.Api.Endpoints.Asset;
using Academy.Asset.Api.Endpoints.Tag;
using Academy.Asset.Api.Infrastructure;
using Academy.Asset.Api.Infrastructure.Database;
using Academy.Asset.Api.Repositories.Asset;
using Academy.Asset.Api.Repositories.Tag;
using Academy.Asset.Api.Validators;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMvc();
builder.Services.AddAcademyProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<AssetDto>, AssetValidator>();
builder.Services.AddScoped<IValidator<TagDto>, TagValidator>();

builder.Services.AddScoped<IAssetRepository, EFCoreAssetRepository>();
builder.Services.AddScoped<ITagRepository, EFCoreTagRepository>();

builder.Services.AddDbContext<AcademyContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("AcademyContext"));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseProblemDetails();

app.MapAssetEndpoints();
app.MapTagEndpoints();

app.Run();