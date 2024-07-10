using Academy.Asset.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Academy.Asset.Api.Repositories.Asset;

public class EFCoreAssetRepository : IAssetRepository
{
    private readonly AcademyContext _context;

    public EFCoreAssetRepository(AcademyContext context)
    {
        _context = context;
    }

    public async Task<Domain.Asset?> GetAssetAsync(Guid id)
    {
        return await _context.Assets.FindAsync(id);
    }

    public async Task<List<Domain.Asset>> GetAssets()
    {
        return await _context.Assets.ToListAsync();
    }

    public async Task AddAssetAsync(Domain.Asset asset)
    {
        _context.Assets.Add(asset);

        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsset(Domain.Asset asset)
    {
        _context.Assets.Remove(asset);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAssetAsync(Domain.Asset asset)
    {
        _context.Assets.Update(asset);

        await _context.SaveChangesAsync();
    }
}