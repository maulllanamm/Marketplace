namespace Repositories.Base
{
    public interface IRepository<Entity> where Entity : class
    {
        Task<Entity> Create(Entity entity);
        Task<int> CreateBulk(List<Entity> entites);
        void Delete(Entity entity);
        void Delete(int id);
        void Edit(Entity entity);

        //read side (could be in separate Readonly Generic Repository)
        Entity GetById(int id);
        IEnumerable<Entity> Filter();
        IEnumerable<Entity> Filter(Func<Entity, bool> predicate);

        //separate method SaveChanges can be helpful when using this pattern with UnitOfWork
        void SaveChanges();
    }
}
