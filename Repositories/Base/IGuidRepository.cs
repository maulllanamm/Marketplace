using Marketplace.Enitities.Base;

namespace Repositories.Base
{
    public interface IGuidRepository<GuidEntity> where GuidEntity : class
    {
        Task<GuidEntity> Create(GuidEntity entity);
        Task<int> CreateBulk(List<GuidEntity> entites);
        Task<Guid> Delete(Guid id);
        Task<int> DeleteBulk(List<GuidEntity> entites);
        Task<Guid> SoftDelete(Guid id);
        Task<int> SoftDeleteBulk(List<GuidEntity> entites);
        Task<GuidEntity> Update(GuidEntity entity);
        Task<int> UpdateBulk(List<GuidEntity> entites);
        Task<List<GuidEntity>> GetAll();
        Task<GuidEntity> GetById(Guid id);
        IEnumerable<GuidEntity> Filter();
        IEnumerable<GuidEntity> Filter(Func<GuidEntity, bool> predicate);

        //separate method SaveChanges can be helpful when using this pattern with UnitOfWork
        void SaveChanges();
    }
}
