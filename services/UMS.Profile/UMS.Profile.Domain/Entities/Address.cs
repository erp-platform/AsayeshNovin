using Core.Domain.Entities;

namespace UMS.Profile.Domain.Entities;

public class Address : BaseEntity
{
    public required string Text { get; set; }
    public int? PostalCode { get; set; }
    public string? Phone { get; set; }
    public virtual required Province Province { get; set; }
    public required string City { get; set; }
}