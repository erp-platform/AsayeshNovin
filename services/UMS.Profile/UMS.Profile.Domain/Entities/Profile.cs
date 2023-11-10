using Core.Domain.Entities;
using UMS.Authentication.Domain.Entities;

namespace UMS.Profile.Domain.Entities;

public class Profile : BaseEntity
{
    public required string Forename { get; set; }
    public required string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string? NationalCode { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? FatherName { get; set; }
    
    public virtual required User User { get; set; }
    
    public virtual IEnumerable<Address> Addresses { get; set; } = new List<Address>();
}