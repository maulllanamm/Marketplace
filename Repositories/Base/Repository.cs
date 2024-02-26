using Marketplace.Repositories;
using Marketplace.Enitities.Base;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Entities.Base;
using Repositories.ConfigUoW;
using Marketplace.Enitities;
using System.Diagnostics;

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

        public async Task<int> CreateBulk(List<Entity> entities)
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

            var splitSize = 10000;
            var stopwatch = new Stopwatch();

            using (var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    if (entities.Count >= 100000)
                    {
                        splitSize *= 2;
                    }

                    var batchs = entities
                                .Select((entities, index) => (entities, index))
                                .GroupBy(pair => pair.index / splitSize)
                                .Select(group => group.Select(pair => pair.entities).ToList())
                                .ToList();

                    foreach (var batch in batchs)
                    {
                        await _context.BulkInsertAsync(batch);
                    }


                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw; // Re-throw exception untuk menyebar ke lapisan yang lebih tinggi jika perlu
                }
            }
            return entities.Count();
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
