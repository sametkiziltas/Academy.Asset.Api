using Npgsql;

namespace Academy.Asset.Api.Repositories.Asset;

public class PostgreSqlAssetRepository : IAssetRepository
{
    private string _connectionString;

    public PostgreSqlAssetRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:AcademyContext"];
    }

    public async Task<Domain.Asset?> GetAssetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Domain.Asset>> GetAssets()
    {
        throw new NotImplementedException();
    }

    public async Task AddAssetAsync(Domain.Asset asset)
    {
        await using var dataSource = NpgsqlDataSource.Create(_connectionString);

        string sql = """INSERT INTO "asset" ("id", "category", "brand", "model", "serialno", "status") VALUES ($1, $2, $3, $4, $5, $6)""";

        await using var cmd = dataSource.CreateCommand(sql);
        
        cmd.Parameters.AddWithValue(asset.Id);
        cmd.Parameters.AddWithValue(asset.Category);
        cmd.Parameters.AddWithValue(asset.Brand);
        cmd.Parameters.AddWithValue(asset.Model);
        cmd.Parameters.AddWithValue(asset.SerialNo);
        cmd.Parameters.AddWithValue(asset.Status.ToString());
            
        var result = await cmd.ExecuteNonQueryAsync();
        
        Console.WriteLine(result.ToString());
    }

    public Task RemoveAsset(Domain.Asset asset)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAssetAsync(Domain.Asset asset)
    {
        throw new NotImplementedException();
    }
}