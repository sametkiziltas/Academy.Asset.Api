namespace Academy.Asset.Api.Repositories.Tag;

using Domain;

public interface ITagRepository
{
    Tag? GetTag(Guid id);
    List<Tag> GetTags();
    void AddTag(Tag tag);
    void RemoveTag(Tag tag);
}