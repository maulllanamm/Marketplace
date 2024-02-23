using Entities.Base;
using Marketplace.Enitities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Base;
using Repositories.ConfigUoW;
using System.Data;
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

        public async Task<Guid> Delete(GuidEntity entity)
        {
            using(var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    _context.Set<GuidEntity>().Remove(entity);
                    unitOfWork.SaveChanges();

                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            return entity.id;
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
                        unitOfWork.SaveChanges();

                        unitOfWork.Commit();
                  
                    }
                    catch (Exception)
                    {
                        unitOfWork.Rollback();
                        throw;
                    }
                }
            }
            return id;
        }

        public async Task<GuidEntity> Edit(GuidEntity entity)
        {
            using (var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    var editedEntity = _context.Set<GuidEntity>().FirstOrDefault(e => e.id == entity.id);
                    if (editedEntity == null)
                    {
                        throw new Exception("Entity not found");
                    }

                    // Mengupdate nilai properti dari editedEntity dengan nilai dari entity
                    _context.Entry(editedEntity).CurrentValues.SetValues(entity);

                    editedEntity.modified_date = DateTime.UtcNow;
                    editedEntity.created_date = DateTime.UtcNow;
                    editedEntity.created_by ??= "system";
                    editedEntity.modified_by ??= "system";
                    editedEntity.is_deleted ??= false;

                    unitOfWork.SaveChanges();
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            return entity;
        }


        public GuidEntity GetById(Guid id)
        {
            return _context.Set<GuidEntity>().FirstOrDefault(e => e.id == id);
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

        //public virtual async Task<List<GuidEntity>> GetAll()
        //{
        //    return await _context.Set<GuidEntity>().ToListAsync();
        //}

        //public virtual async Task<GuidEntity> Get(Guid id)
        //{
        //    return await _context.Set<GuidEntity>().FindAsync(id);
        //}

        //public virtual async Task Create(GuidEntity item)
        //{
        //    item.id = Guid.NewGuid();
        //    item.created_date = DateTime.UtcNow;
        //    item.modified_date = DateTime.UtcNow;
        //    item.is_deleted = false;
        //    if (item.created_by == null || item.created_by == string.Empty)
        //    {
        //        item.created_by = "system";
        //    }
        //    if (item.modified_by == null || item.modified_by == string.Empty)
        //    {
        //        item.modified_by = "system";
        //    }
        //    await _dbSet.AddAsync(item);
        //    await _context.SaveChangesAsync();
        //}

        //public virtual async void CreateBulk(List<GuidEntity> items)
        //{
        //    items = items
        //        .Select(x =>
        //        {
        //            x.id = x.id == null || x.id == Guid.Empty ? Guid.NewGuid() : x.id;
        //            x.is_deleted ??= false;
        //            x.created_by ??= "system";
        //            x.modified_by ??= "system";
        //            return x;
        //        })
        //        .ToList();
        //    _context.AddRangeAsync(items);
        //    await _context.BulkSaveChangesAsync();
        //}


        //public virtual async void Update(GuidEntity item)
        //{
        //    _context.Update(item);
        //    await _context.SaveChangesAsync();
        //}

        //public virtual async void Delete(Guid id)
        //{
        //    var item = await _context.Set<GuidEntity>().FindAsync(id);
        //    _context.Remove(item);
        //    await _context.SaveChangesAsync();
        //}
    }
}
