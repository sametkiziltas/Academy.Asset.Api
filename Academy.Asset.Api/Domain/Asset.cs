using Academy.Asset.Api.Domain.Enums;

namespace Academy.Asset.Api.Domain;

public class Asset
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string SerialNo { get; set; }
    public Status Status { get; set; }
    
    public Tag? Tag { get; set; }
    
}