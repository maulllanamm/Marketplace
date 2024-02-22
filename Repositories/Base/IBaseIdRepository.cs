namespace Repositories.Base
{
    public interface IBaseIdRepository<Entity> where Entity : class
    {
        Task<List<Entity>> GetAll();
        Task<Entity> Get(long id);
        void Create(Entity entity);
        void Update(Entity entity);
        void Delete(long id);
    }
}
