using Academy.Asset.Api.Infrastructure.Database;
using Academy.Asset.Api.Repositories.Asset;
using Microsoft.EntityFrameworkCore;

namespace Academy.Asset.Api.Repositories.Tag;

public class EFCoreTagRepository : ITagRepository
{
    private readonly AcademyContext _context;

    public EFCoreTagRepository(AcademyContext context)
    {
        _context = context;
    }

    public async Task<Domain.Tag?> GetTagAsync(Guid id)
    {
        return await _context.Tags.FindAsync(id);
    }

    public async Task<List<Domain.Tag>> GetTags()
    {
        return await _context.Tags.ToListAsync();
    }

    public async Task AddTagAsync(Domain.Tag tag)
    {
        _context.Tags.Add(tag);

        await _context.SaveChangesAsync();
    }

    public async Task RemoveTagAsync(Domain.Tag tag)
    {
        _context.Tags.Remove(tag);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateTagAsync(Domain.Tag tag)
    {
        _context.Tags.Update(tag);

        await _context.SaveChangesAsync();
    }
}