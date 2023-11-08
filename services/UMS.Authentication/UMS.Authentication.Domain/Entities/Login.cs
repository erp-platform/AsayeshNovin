using Core.Domain.Entities;

namespace UMS.Authentication.Domain.Entities;

public class Login : BaseEntity
{
    public required string Ip { get; set; }
    public bool IsSuccess { get; set; }
}