namespace Academy.Asset.Api.Repositories.Asset;

using Domain;

public class InMemoryAssetRepository : IAssetRepository
{
    private readonly List<Asset> _assets = new List<Asset>();

    public Task<Asset?> GetAssetAsync(Guid id)
    {
        return Task.FromResult(_assets.Find(x => x.Id == id));
    }

    public Task<List<Asset>> GetAssets()
    {
        return Task.FromResult(_assets);
    }

    public Task AddAssetAsync(Asset asset)
    {
        _assets.Add(asset);
        
        return Task.CompletedTask;
    }

    public Task RemoveAsset(Asset asset)
    {
        _assets.Remove(asset);
        
        return Task.CompletedTask;
    }

    public Task UpdateAssetAsync(Asset asset)
    {
        throw new NotImplementedException();
    }
}