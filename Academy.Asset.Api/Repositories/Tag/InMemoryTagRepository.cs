namespace Academy.Asset.Api.Repositories.Tag;

using Domain;

public class InMemoryTagRepository : ITagRepository
{
    private readonly List<Tag> _tags = new();

    public Tag? GetTag(Guid id)
    {
        return _tags.Find(x => x.Id == id);
    }

    public List<Tag> GetTags()
    {
        return _tags;
    }

    public void AddTag(Tag tag)
    {
        _tags.Add(tag);
    }

    public void RemoveTag(Tag tag)
    {
        _tags.Remove(tag);
    }
}