using Marketplace.Repositories;
using Marketplace.Enitities.Base;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Entities.Base;

namespace Marketplace.Repositories.Base
{
    public class Repository<Entity> : IRepository<Entity>
        where Entity : class, IEntity
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public void Create(Entity entity)
        {
            _context.Set<Entity>().Add(entity);
        }

        public void Delete(Entity entity)
        {
            _context.Set<Entity>().Remove(entity);
        }

        public void Delete(int id)
        {
            var entityToDelete = _context.Set<Entity>().FirstOrDefault(e => e.id == id);
            if (entityToDelete != null)
            {
                _context.Set<Entity>().Remove(entityToDelete);
            }
        }

        public void Edit(Entity entity)
        {
            var editedEntity = _context.Set<Entity>().FirstOrDefault(e => e.id == entity.id);
            editedEntity = entity;
        }

        public Entity GetById(int id)
        {
            return _context.Set<Entity>().FirstOrDefault(e => e.id == id);
        }

        public IEnumerable<Entity> Filter()
        {
            return _context.Set<Entity>();
        }

        public IEnumerable<Entity> Filter(Func<Entity, bool> predicate)
        {
            return _context.Set<Entity>().Where(predicate);
        }

        public void SaveChanges() => _context.SaveChanges();
    }
}
