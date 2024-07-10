namespace Academy.Asset.Api.Repositories.Asset;

using Domain;

public interface IAssetRepository
{
    Task<Asset?> GetAssetAsync(Guid id);
    Task<List<Asset>> GetAssets();
    Task AddAssetAsync(Asset asset);
    Task RemoveAsset(Asset asset);
    Task UpdateAssetAsync(Asset asset);
}