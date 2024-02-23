namespace Repositories.Base
{
    public interface IGuidRepository<GuidEntitiy> where GuidEntitiy : class
    {
        Task Create(GuidEntitiy entity);
        void Delete(GuidEntitiy entity);
        void Delete(Guid id);
        void Edit(GuidEntitiy entity);

        //read side (could be in separate Readonly Generic Repository)
        GuidEntitiy GetById(Guid id);
        IEnumerable<GuidEntitiy> Filter();
        IEnumerable<GuidEntitiy> Filter(Func<GuidEntitiy, bool> predicate);

        //separate method SaveChanges can be helpful when using this pattern with UnitOfWork
        void SaveChanges();
    }
}
