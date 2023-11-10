using Core.Domain.Entities;

namespace UMS.Profile.Domain.Entities;

public class Country : BaseEntity
{
    public required string Key { get; set; }
    public required string Name { get; set; }
    
    public virtual IEnumerable<Province> Provinces { get; set; } = new List<Province>();
}