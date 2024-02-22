namespace Repositories.Base
{
    public interface IBaseGuidRepository<Entity> where Entity : class
    {
        Task<List<Entity>> GetAll();
        Task<Entity> Get(Guid id);
        void Create(Entity entity);
        void Update(Entity entity);
        void Delete(Guid id);
    }
}
