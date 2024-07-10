using Academy.Asset.Api.Domain;
using Academy.Asset.Api.Domain.Enums;
using Academy.Asset.Api.Dtos;
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

builder.Services.AddScoped<IValidator<AssetDto>, AssetValidator>();
builder.Services.AddScoped<IValidator<TagDto>, TagValidator>();

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

app.MapPost("/assets", async (IValidator<AssetDto> validator, AssetDto assetDto) =>
{
    var validationResult = await validator.ValidateAsync(assetDto);

    if (!validationResult.IsValid) {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    if (!tags.Any(x => x.Id == assetDto.TagId))
    {
        return Results.NotFound("Tag not found");
    }

    var asset = new Asset()
    {
        Id = Guid.NewGuid(),
        Category = assetDto.Category,
        Brand = assetDto.Brand,
        Model = assetDto.Model,
        SerialNo = assetDto.SerialNo,
        Status = assetDto.Status,
        TagId = assetDto.TagId
    };
    
    assets.Add(asset);
    return Results.Created($"/assets/{asset.Id}", asset);
});

app.MapPut("/assets/{id}", async (IValidator<AssetDto> validator, Guid id, AssetDto assetDto) =>
{
    var validationResult = await validator.ValidateAsync(assetDto);

    if (!validationResult.IsValid) {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    
    if (!tags.Any(x => x.Id == assetDto.TagId))
    {
        return Results.NotFound("Tag not found");
    }
    
    var existingAsset = assets.FirstOrDefault(a => a.Id == id);

    if (existingAsset is null)
    {
        return Results.NotFound();
    }
    
    BusinessException.ThrowIfTrue(ErrorMessages.AssetMustNotBeDown, existingAsset.Status == Status.Down && existingAsset.Tag.Id != assetDto.TagId);

    existingAsset.Category = assetDto.Category;
    existingAsset.Brand = assetDto.Brand;
    existingAsset.Model = assetDto.Model;
    existingAsset.SerialNo = assetDto.SerialNo;
    existingAsset.Status = assetDto.Status;
    existingAsset.TagId = assetDto.TagId;

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

app.MapPost("/tags", async (IValidator<TagDto> validator, TagDto tagDto) =>
{
    var validationResult = await validator.ValidateAsync(tagDto);

    if (!validationResult.IsValid) {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var tag = new Tag()
    {
        Id = Guid.NewGuid(),
        MacAddress = tagDto.MacAddress,
        Name = tagDto.Name
    };
    
    tags.Add(tag);
    return Results.Created($"/tags/{tag.Id}", tagDto);
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