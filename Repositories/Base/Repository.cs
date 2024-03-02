using Marketplace.Repositories;
using Marketplace.Enitities.Base;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Entities.Base;
using Repositories.ConfigUoW;
using Marketplace.Enitities;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Linq.Expressions;

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
        public async Task<int> Count()
        {
            return await _context.Set<Entity>().CountAsync();
        }

        public async Task<List<Entity>> GetAll(int page)
        {
            var pageResult = 5f;
            var countProduct = await Count();
            var totalPage = Math.Ceiling(countProduct / pageResult);

            var items = await _context.Set<Entity>()
                                      .Skip((page - 1) * (int)pageResult)
                                      .Take((int)pageResult)
                                      .ToListAsync();

            return items;
        }

        public async Task<List<Entity>> GetAll()
        {
            return _context.Set<Entity>().AsNoTracking().ToList();
        }



        public async Task<List<Entity>> GetByListId(List<int> listId)
        {
            return _context.Set<Entity>().Where(e => listId.Contains(e.id)).ToList();
        }

        public async Task<List<Entity>> GetByListProperty(string field, string[] values)
        {
            // Membuat parameter ekspresi
            // Pastikan 'Entity' adalah nama model entitas yang sesuai
            IQueryable<Entity> query = _context.Set<Entity>();

            // Filter berdasarkan properti 'field' dan nilai-nilai 'values'
            query = query.Where(e => values.Contains(EF.Property<string>(e, field)));

            // Eksekusi query dan ambil hasilnya
            List<Entity> result = await query.ToListAsync();

            return result;
        }


        public async Task<Entity> GetById(int id)
        {
            return _context.Set<Entity>().FirstOrDefault(e => e.id == id);
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


        public async Task<Entity> Update(Entity entity)
        {
            using (var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    var editedEntity = _context.Set<Entity>().FirstOrDefault(e => e.id == entity.id);

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

        public async Task<int> UpdateBulk(List<Entity> entities)
        {
            using (var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    _context.Set<Entity>().BulkUpdate(entities);
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

        public async Task<int> Delete(int id)
        {
            var entityToDelete = _context.Set<Entity>().FirstOrDefault(e => e.id == id);
            if (entityToDelete != null)
            {
                using (var unitOfWork = new UnitOfWork(_context))
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        _context.Set<Entity>().Remove(entityToDelete);
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

        public async Task<int> DeleteBulk(List<Entity> entities)
        {
            using (var unitOfWork = new UnitOfWork(_context))
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    _context.Set<Entity>().BulkDelete(entities);
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

        public async Task<int> SoftDelete(int id)
        {
            var entityToDelete = _context.Set<Entity>().FirstOrDefault(e => e.id == id);
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

        public async Task<int> SoftDeleteBulk(List<Entity> entities)
        {
            var listId = entities.Select(x => x.id).ToList();
            var entitiesToDelete = _context.Set<Entity>().Where(x => listId.Contains(x.id)).ToList();
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
