namespace Academy.Asset.Api.Repositories.Asset;

using Domain;

public class InMemoryAssetRepository : IAssetRepository
{
    private readonly List<Asset> _assets = new List<Asset>();

    public Asset? GetAsset(Guid id)
    {
        return _assets.Find(x => x.Id == id);
    }

    public List<Domain.Asset> GetAssets()
    {
        return _assets;
    }

    public void AddAsset(Asset asset)
    {
        _assets.Add(asset);
    }

    public void RemoveAsset(Asset asset)
    {
        _assets.Remove(asset);
    }
}