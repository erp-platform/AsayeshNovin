using System.ComponentModel.DataAnnotations.Schema;

namespace UMS.Authentication.Domain.Entities;

[Table("Channels")]
public class Channel : BaseEntity
{
    // Autoincrement Id
    public required int AId { get; set; }
    public required string? Name { get; set; }
}