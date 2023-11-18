using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Application.UoW.InterfaceUoWAuthentication;

namespace UMS.Authentication.Application.UoW.ImplementionUoWAuthentication;

public class UnitOfWorkAuthentication : IUnitOfWorkAuthentication
{
    DbContext _context;
    IDbContextTransaction _transaction;


    public UnitOfWorkAuthentication(AppDbContext context)
    {
        _context = context;
        _transaction = _context.Database.BeginTransaction();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void Commit()
    {
        _transaction.Commit();
    }

    public void Rollback()
    {
        _transaction.Rollback();
    }
    protected virtual void Dispose(bool disposing)
    {
        _context?.Dispose();
    }
    public IAuthService AuthService()
    {
        throw new NotImplementedException();
    }
}