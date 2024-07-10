using Academy.Asset.Api.Domain.Enums;
using Academy.Asset.Api.Dtos;
using Academy.Asset.Api.Infrastructure;
using Academy.Asset.Api.Repositories.Asset;
using Academy.Asset.Api.Repositories.Tag;
using FluentValidation;

namespace Academy.Asset.Api.Endpoints.Asset;

using Domain;

public static class AssetEndpoints
{
    public static void MapAssetEndpoints(this WebApplication app)
    {
        app.MapGet(
                "/assets",
                (IAssetRepository repository) => { return repository.GetAssets(); })
            .WithName("Assets")
            .WithOpenApi();

        app.MapGet(
            "/assets/{id}",
            (Guid id, IAssetRepository repository) => { return repository.GetAssetAsync(id); });

        app.MapPost(
            "/assets",
            async (IValidator<AssetDto> validator, AssetDto assetDto, IAssetRepository assetRepository, ITagRepository tagRepository) =>
            {
                var validationResult = await validator.ValidateAsync(assetDto);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                if (assetDto.TagId.HasValue)
                {
                    Tag? tag = await tagRepository.GetTagAsync(assetDto.TagId.Value);

                    if (tag is null)
                    {
                        return Results.NotFound("Tag not found");
                    }
                }

                var asset = new Domain.Asset()
                {
                    Id = Guid.NewGuid(),
                    Category = assetDto.Category,
                    Brand = assetDto.Brand,
                    Model = assetDto.Model,
                    SerialNo = assetDto.SerialNo,
                    Status = assetDto.Status,
                    TagId = assetDto.TagId
                };

                await assetRepository.AddAssetAsync(asset);

                return Results.Created($"/assets/{asset.Id}", asset);
            });

        app.MapPut(
            "/assets/{id}",
            async (
                IValidator<AssetDto> validator,
                Guid id,
                AssetDto assetDto,
                IAssetRepository assetRepository,
                ITagRepository tagRepository) =>
            {
                var validationResult = await validator.ValidateAsync(assetDto);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                if (assetDto.TagId.HasValue)
                {
                    Tag? tag = await tagRepository.GetTagAsync(assetDto.TagId.Value);

                    if (tag is null)
                    {
                        return Results.NotFound("Tag not found");
                    }
                }

                var existingAsset = await assetRepository.GetAssetAsync(id);
                if (existingAsset is null)
                {
                    return Results.NotFound();
                }

                BusinessException.ThrowIfTrue(
                    ErrorMessages.AssetMustNotBeDown,
                    existingAsset.Status == Status.Down && existingAsset.Tag.Id != assetDto.TagId);

                existingAsset.Category = assetDto.Category;
                existingAsset.Brand = assetDto.Brand;
                existingAsset.Model = assetDto.Model;
                existingAsset.SerialNo = assetDto.SerialNo;
                existingAsset.Status = assetDto.Status;
                existingAsset.TagId = assetDto.TagId;

                await assetRepository.UpdateAssetAsync(existingAsset);

                return Results.Ok(existingAsset);
            });

        app.MapDelete(
            "/assets/{id}",
            async (Guid id, IAssetRepository assetRepository) =>
            {
                Asset? existingAsset = await assetRepository.GetAssetAsync(id);

                if (existingAsset is null)
                {
                    return Results.NotFound();
                }

                assetRepository.RemoveAsset(existingAsset);

                return Results.NoContent();
            });
    }
}