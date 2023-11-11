using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Entities;

namespace UMS.Authentication.Domain.Entities;

[Table("Channels")]
public class Channel : BaseEntity
{
    // Autoincrement Id
    public required int AId { get; set; }
    public required string? Name { get; set; }
}