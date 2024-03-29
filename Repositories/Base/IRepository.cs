﻿using Marketplace.Enitities.Base;

namespace Repositories.Base
{
    public interface IRepository<Entity> where Entity : class
    {
        Task<Entity> Create(Entity entity);
        Task<int> CreateBulk(List<Entity> entites);
        Task<int> Delete(int id);
        Task<int> DeleteBulk(List<Entity> entites);        
        Task<int> SoftDelete(int id);
        Task<int> SoftDeleteBulk(List<Entity> entites);
        Task<Entity> Update(Entity entity);
        Task<int> UpdateBulk(List<Entity> entites);
        Task<List<Entity>> GetAll() ;
        Task<List<Entity>> GetAll(int page);
        Task<List<Entity>> GetByListId(List<int> listId);
        Task<List<Entity>> GetByListProperty(string field, string[] values);
        Task<Entity> GetById(int id);
        IEnumerable<Entity> Filter();
        IEnumerable<Entity> Filter(Func<Entity, bool> predicate);

        //separate method SaveChanges can be helpful when using this pattern with UnitOfWork
        void SaveChanges();
        Task<int> Count();
    }
}
