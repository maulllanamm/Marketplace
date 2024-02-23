using Entities.Base;
using Repositories.Base;

namespace Repositories.ConfigUoW
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Entity> GetRepository<Entity>() where Entity : class, IEntity;
        IGuidRepository<GuidEntity> GetGuidRepository<GuidEntity>() where GuidEntity : class, IGuidEntity;
        void BeginTransaction();
        void Commit();
        void Rollback();
        int SaveChanges();
    }
}
