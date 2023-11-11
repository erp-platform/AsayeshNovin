using Core.Domain.Entities;

namespace UMS.Profile.Domain.Entities;

public class Province : BaseEntity
{
    public required string Key { get; set; }
    public required string Name { get; set; }

    public required Guid CountryId { get; set; }

    public virtual IEnumerable<Address> Addresses { get; set; } = new List<Address>();
}