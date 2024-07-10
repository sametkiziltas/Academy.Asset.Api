using Academy.Asset.Api.Dtos;
using Academy.Asset.Api.Repositories.Tag;
using FluentValidation;

namespace Academy.Asset.Api.Endpoints.Tag;

public static class TagEndpoints
{
    public static void MapTagEndpoints(this WebApplication app)
    {
        app.MapGet("/tags",
            async (ITagRepository repository) =>
        {
            return await repository.GetTags();
        });

        app.MapGet("/tags/{id}",
            async (Guid id, ITagRepository repository) =>
        {
            return await repository.GetTagAsync(id);
        });

        app.MapPost("/tags", async (IValidator<TagDto> validator, TagDto tagDto, ITagRepository repository) =>
        {
            var validationResult = await validator.ValidateAsync(tagDto);

            if (!validationResult.IsValid) {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var tag = new Domain.Tag()
            {
                Id = Guid.NewGuid(),
                MacAddress = tagDto.MacAddress,
                Name = tagDto.Name
            };
    
            await repository.AddTagAsync(tag);
            
            return Results.Created($"/tags/{tag.Id}", tagDto);
        });

        app.MapDelete("/tags/{id}",
            async (Guid id, ITagRepository repository) =>
        {
            var tag = await repository.GetTagAsync(id);
            if (tag is null)
            {
                return Results.NotFound();
            }

            await repository.RemoveTagAsync(tag);
            
            return Results.NoContent();
        });
    }
}