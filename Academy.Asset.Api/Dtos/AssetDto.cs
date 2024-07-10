using Academy.Asset.Api.Domain.Enums;

namespace Academy.Asset.Api.Dtos;

public class AssetDto
{
    public string Category { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string SerialNo { get; set; }
    public Status Status { get; set; }
    public Guid? TagId { get; set; }
}