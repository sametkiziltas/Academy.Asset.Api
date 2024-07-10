namespace Academy.Asset.Api.Repositories.Tag;

using Domain;

public interface ITagRepository
{
    Task<Tag?> GetTagAsync(Guid id);
    Task<List<Tag>> GetTags();
    Task AddTagAsync(Tag tag);
    Task RemoveTagAsync(Tag tag);
    Task UpdateTagAsync(Tag tag);
}