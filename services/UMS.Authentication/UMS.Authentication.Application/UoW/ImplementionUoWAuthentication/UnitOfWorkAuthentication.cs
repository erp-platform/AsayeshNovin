using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Application.UoW.InterfaceUoWAuthentication;

namespace UMS.Authentication.Application.UoW.ImplementionUoWAuthentication;

public class UnitOfWorkAuthentication : IUnitOfWorkAuthentication
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void Commit()
    {
        throw new NotImplementedException();
    }

    public void Rollback()
    {
        throw new NotImplementedException();
    }

    public IAuthService AuthService()
    {
        throw new NotImplementedException();
    }
}