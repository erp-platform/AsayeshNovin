using Core.Infrastructure.Repositories;


namespace UMS.Authentication.Application.UoW;

public interface IUnitOfWorkAuthentication : IDisposable
{
    void Commit();
    void Rollback();
    IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class;
}