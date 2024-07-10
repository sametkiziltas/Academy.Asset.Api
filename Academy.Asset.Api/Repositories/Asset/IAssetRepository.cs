namespace Academy.Asset.Api.Repositories.Asset;

using Domain;

public interface IAssetRepository
{
    Asset? GetAsset(Guid id);
    List<Asset> GetAssets();
    void AddAsset(Asset asset);
    void RemoveAsset(Asset asset);
}