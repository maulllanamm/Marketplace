using Entities.Base;
using Marketplace.Enitities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Base;
using Repositories.ConfigUoW;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Marketplace.Repositories.Base
{
    public class GuidRepository<GuidEntity> : IGuidRepository<GuidEntity>
        where GuidEntity : class, IGuidEntity

    {
        protected readonly DataContext _context;
        public GuidRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<GuidEntity>> GetAll()
        {
            return _context.Set<GuidEntity>().AsNoTracking().ToList();
        }

        public async Task<GuidEntity> GetById(Guid id)
        {
            return _context.Set<GuidEntity>().FirstOrDefault(e => e.id == id);
        }

        public async Task<GuidEntity> Create(GuidEntity entity)
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

                    _context.Set<GuidEntity>().Add(entity);
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

        public async Task<int> CreateBulk(List<GuidEntity> entities)
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


        public async Task<GuidEntity> Update(GuidEntity entity)
        {
            using (var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    var editedEntity = _context.Set<GuidEntity>().FirstOrDefault(e => e.id == entity.id);

                    if (editedEntity != null)
                    {
                        // Update properti dari editedEntity dengan nilai dari entity yang baru
                        _context.Entry(editedEntity).CurrentValues.SetValues(entity);

                        unitOfWork.BeginTransaction();
                        _context.SaveChanges();
                        unitOfWork.Commit();
                    }
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw; // Re-throw exception untuk menyebar ke lapisan yang lebih tinggi jika perlu
                }
            }


            return entity;
        }

        public async Task<int> UpdateBulk(List<GuidEntity> entities)
        {
            using (var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    _context.Set<GuidEntity>().BulkUpdate(entities);
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

        public async Task<Guid> Delete(Guid id)
        {
            var entityToDelete = _context.Set<GuidEntity>().FirstOrDefault(e => e.id == id);
            if (entityToDelete != null)
            {
                using (var unitOfWork = new UnitOfWork(_context))
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        _context.Set<GuidEntity>().Remove(entityToDelete);
                        unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                        unitOfWork.Rollback();
                        throw; // Re-throw exception untuk menyebar ke lapisan yang lebih tinggi jika perlu
                    }
                }
            }

            return id;
        }

        public async Task<int> DeleteBulk(List<GuidEntity> entities)
        {
            using (var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    _context.Set<GuidEntity>().BulkDelete(entities);
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

        public async Task<Guid> SoftDelete(Guid id)
        {
            var entityToDelete = _context.Set<GuidEntity>().FirstOrDefault(e => e.id == id);
            if (entityToDelete != null)
            {
                using (var unitOfWork = new UnitOfWork(_context))
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        entityToDelete.is_deleted = true;
                        _context.SaveChanges();
                        unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                        unitOfWork.Rollback();
                        throw; // Re-throw exception untuk menyebar ke lapisan yang lebih tinggi jika perlu
                    }
                }
            }

            return id;
        }

        public async Task<int> SoftDeleteBulk(List<GuidEntity> entities)
        {
            var listId = entities.Select(x => x.id).ToList();
            var entitiesToDelete = _context.Set<GuidEntity>().Where(x => listId.Contains(x.id)).ToList();
            if (entitiesToDelete != null)
            {
                using (var unitOfWork = new UnitOfWork(_context))
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        entitiesToDelete = entitiesToDelete.Select(x =>
                        {
                            x.is_deleted = true;
                            return x;
                        }).ToList();
                        _context.SaveChanges();
                        unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                        unitOfWork.Rollback();
                        throw; // Re-throw exception untuk menyebar ke lapisan yang lebih tinggi jika perlu
                    }
                }
            }
            return entitiesToDelete.Count();
        }

        public IEnumerable<GuidEntity> Filter()
        {
            return _context.Set<GuidEntity>();
        }

        public IEnumerable<GuidEntity> Filter(Func<GuidEntity, bool> predicate)
        {
            return _context.Set<GuidEntity>().Where(predicate);
        }

        public void SaveChanges() => _context.SaveChanges();
    }
}
