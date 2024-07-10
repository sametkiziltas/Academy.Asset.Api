namespace Academy.Asset.Api.Repositories.Tag;

using Domain;

public class InMemoryTagRepository : ITagRepository
{
    private readonly List<Tag> _tags = new();

    public Task<Tag?> GetTagAsync(Guid id)
    {
        return Task.FromResult(_tags.Find(x => x.Id == id));
    }

    public Task<List<Tag>> GetTags()
    {
        return Task.FromResult(_tags);
    }

    public Task AddTagAsync(Tag tag)
    {
        _tags.Add(tag);

        return Task.CompletedTask;
    }

    public Task RemoveTagAsync(Tag tag)
    {
        _tags.Remove(tag);

        return Task.CompletedTask;
    }

    public Task UpdateTagAsync(Tag tag)
    {
        throw new NotImplementedException();
    }
}