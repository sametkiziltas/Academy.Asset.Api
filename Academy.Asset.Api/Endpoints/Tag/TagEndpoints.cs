using Academy.Asset.Api.Dtos;
using Academy.Asset.Api.Repositories.Tag;
using FluentValidation;

namespace Academy.Asset.Api.Endpoints.Tag;

public static class TagEndpoints
{
    public static void MapTagEndpoints(this WebApplication app)
    {
        app.MapGet("/tags", (ITagRepository repository) =>
        {
            return repository.GetTags();
        });

        app.MapGet("/tags/{id}", (Guid id, ITagRepository repository) =>
        {
            return repository.GetTag(id);
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
    
            repository.AddTag(tag);
            
            return Results.Created($"/tags/{tag.Id}", tagDto);
        });

        app.MapDelete("/tags/{id}", (Guid id, ITagRepository repository) =>
        {
            var tag = repository.GetTag(id);
            if (tag is null)
            {
                return Results.NotFound();
            }

            repository.RemoveTag(tag);
            
            return Results.NoContent();
        });
    }
}