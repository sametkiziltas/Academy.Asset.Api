using Academy.Asset.Api.Domain;
using Academy.Asset.Api.Domain.Enums;
using Academy.Asset.Api.Infrastructure;
using Academy.Asset.Api.Validators;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMvc();
builder.Services.AddAcademyProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<Asset>, AssetValidator>();
builder.Services.AddScoped<IValidator<Tag>, TagValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseProblemDetails();

List<Tag> tags = new()
{
    new Tag
    {
        Id = Guid.NewGuid(),
        MacAddress = "00:11:22:33:44:55",
        Name = "AssetTag1"
    },
};

var assets = new List<Asset>
{
    new Asset
    {
        Id = Guid.NewGuid(),
        Category = "Laptop",
        Brand = "Dell",
        Model = "Latitude 7400",
        SerialNo = "123456",
        Status = Status.Usable,
        Tag = new Tag
        {
            Id = Guid.NewGuid(),
            MacAddress = "00:11:22:33:44:55",
            Name = "AssetTag1"
        }
    },
    new Asset
    {
        Id = Guid.NewGuid(),
        Category = "Laptop",
        Brand = "Dell",
        Model = "Latitude 7411",
        SerialNo = "123456",
        Status = Status.Down,
        Tag = new Tag
        {
            Id = Guid.NewGuid(),
            MacAddress = "55:44:33:22:11:00",
            Name = "AssetTag2"
        }
    }
};

app.MapGet("/assets", () =>
{
    return assets;
})
.WithName("Assets")
.WithOpenApi();

app.MapGet("/assets/{id}", (Guid id) =>
{
    return assets.FirstOrDefault(a => a.Id == id);
});

app.MapPost("/assets", async (IValidator<Asset> validator, Asset asset) =>
{
    var validationResult = await validator.ValidateAsync(asset);

    if (!validationResult.IsValid) {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    
    asset.Id = Guid.NewGuid();
    assets.Add(asset);
    return Results.Created($"/assets/{asset.Id}", asset);
});

app.MapPut("/assets/{id}", async (IValidator<Asset> validator, Guid id, Asset asset) =>
{
    var validationResult = await validator.ValidateAsync(asset);

    if (!validationResult.IsValid) {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    
    var existingAsset = assets.FirstOrDefault(a => a.Id == id);

    if (existingAsset is null)
    {
        return Results.NotFound();
    }
    
    BusinessException.ThrowIfTrue(ErrorMessages.AssetMustNotBeDown, existingAsset.Status == Status.Down && existingAsset.Tag.Id != asset.Tag.Id);

    existingAsset.Category = asset.Category;
    existingAsset.Brand = asset.Brand;
    existingAsset.Model = asset.Model;
    existingAsset.SerialNo = asset.SerialNo;
    existingAsset.Status = asset.Status;
    existingAsset.Tag = asset.Tag;

    return Results.Ok(existingAsset);
});

app.MapDelete("/assets/{id}", (Guid id) =>
{
    Asset? existingAsset = assets.FirstOrDefault(a => a.Id == id);

    if (existingAsset is null)
    {
        return Results.NotFound();
    }

    assets.Remove(existingAsset);

    return Results.NoContent();
});

app.MapGet("/tags", () =>
{
    return tags;
});

app.MapGet("/tags/{id}", (Guid id) =>
{
    return tags.FirstOrDefault(t => t.Id == id);
});

app.MapPost("/tags", async (IValidator<Tag> validator, Tag tag) =>
{
    var validationResult = await validator.ValidateAsync(tag);

    if (!validationResult.IsValid) {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    tag.Id = Guid.NewGuid();
    tags.Add(tag);
    return Results.Created($"/tags/{tag.Id}", tag);
});

app.MapDelete("/tags/{id}", (Guid id) =>
{
    var tag = tags.FirstOrDefault(t => t.Id == id);
    if (tag is null)
    {
        return Results.NotFound();
    }

    tags.Remove(tag);
    return Results.NoContent();
});

app.Run();