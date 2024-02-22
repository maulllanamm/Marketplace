using Marketplace.Enitities.Base;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Marketplace.Repositories.Base
{
    public class BaseGuidRepository<Entity> : IBaseGuidRepository<Entity>
        where Entity : BaseGuidEntity, new()
    {
        private readonly DataContext _context;

        public BaseGuidRepository(DataContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Entity>> GetAll()
        {
            return await _context.Set<Entity>().ToListAsync();
        }

        public virtual async Task<Entity> Get(Guid id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }

        public virtual async void Create(Entity item)
        {
            item.id = Guid.NewGuid();
            item.created_date = DateTime.UtcNow;
            item.modified_date = DateTime.UtcNow;
            if (item.created_by == null || item.created_by == string.Empty)
            {
                item.created_by = "system";
            }
            if (item.modified_by == null || item.modified_by == string.Empty)
            {
                item.modified_by = "system";
            }
            await _context.Set<Entity>().AddAsync(item);
            await _context.SaveChangesAsync();

        }

        public virtual async void CreateBulk(List<Entity> items)
        {
            items = items
                .Select(x =>
                {
                    x.id = x.id == null || x.id == Guid.Empty ? Guid.NewGuid() : x.id;
                    x.is_deleted ??= false;
                    x.created_by ??= "system";
                    x.modified_by ??= "system";
                    return x;
                })
                .ToList();
            _context.AddRangeAsync(items);
            await _context.BulkSaveChangesAsync();
        }


        public virtual async void Update(Entity item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public virtual async void Delete(Guid id)
        {
            var item = await _context.Set<Entity>().FindAsync(id);
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
