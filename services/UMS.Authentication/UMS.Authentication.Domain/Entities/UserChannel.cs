using System.Text.Json.Serialization;

namespace UMS.Authentication.Domain.Entities;

public class UserChannel : BaseEntity
{
    public required string Value { get; set; }
    public bool IsDefault { get; set; }

    public virtual User? User { get; set; }
    public virtual required Channel Channel { get; set; }
    public virtual Verification? Verification { get; set; }
    [JsonIgnore] public virtual IEnumerable<PasswordReset> PasswordResets { get; set; } = new List<PasswordReset>();
    public virtual IEnumerable<Login> Logins { get; set; } = new List<Login>();
}