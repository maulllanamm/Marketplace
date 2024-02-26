using Marketplace.Repositories;
using Marketplace.Enitities.Base;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Entities.Base;
using Repositories.ConfigUoW;

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

        public async Task<Entity> Create(Entity entity)
        {
            entity.created_date ??= DateTime.UtcNow;
            entity.created_by ??= "system";
            entity.modified_date ??= DateTime.UtcNow;
            entity.modified_by ??= "system";
            entity.is_deleted = false;

            using (var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    _context.Set<Entity>().Add(entity);
                    unitOfWork.SaveChanges();

                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw; // Re-throw exception untuk menyebar ke lapisan yang lebih tinggi jika perlu
                }
            }
            return entity;
        }

        public async Task<List<Entity>> CreateBulk(List<Entity> entities)
        {

            entities = entities.Select(x =>
            {
                x.is_deleted ??= false;
                x.created_by ??= "system";
                x.created_date = DateTime.UtcNow;
                x.modified_by ??= "system";
                x.modified_date = DateTime.UtcNow;
                return x;
            }).ToList();
            using (var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    await _context.Set<Entity>().AddRangeAsync(entities);
                    unitOfWork.SaveChanges();

                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw; // Re-throw exception untuk menyebar ke lapisan yang lebih tinggi jika perlu
                }
            }
            return entities;
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
