using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace UMS.Authentication.Domain.Entities;

[Index(nameof(Username), IsUnique = true)]
public class User : BaseEntity
{
    public required string Username { get; set; }
    [JsonIgnore] public required string Password { get; set; }

    [JsonIgnore] public virtual IEnumerable<UserChannel> Channels { get; set; } = new List<UserChannel>();

    [JsonIgnore] public required Guid VerificationId { get; set; }
}