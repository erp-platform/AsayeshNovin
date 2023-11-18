using Core.Infrastructure.Repositories;
using UMS.Authentication.Application.Interfaces;

namespace UMS.Authentication.Application.UoW.InterfaceUoWAuthentication;

public interface IUnitOfWorkAuthentication : IDisposable
{
    void Save();
    void Commit();
    void Rollback();
    IAuthService AuthService();
}