using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace UMS.Authentication.Domain.Entities;

[Index(nameof(Username), IsUnique = true)]
public class User : BaseEntity
{
    public required string Username { get; set; }
    public required string Password { get; set; }

    public virtual IEnumerable<UserChannel> Channels { get; set; } = new List<UserChannel>();

    public required Guid VerificationId { get; set; }
}