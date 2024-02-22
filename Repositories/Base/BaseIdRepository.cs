using Marketplace.Repositories;
using Marketplace.Enitities.Base;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Marketplace.Repositories.Base
{
    public class BaseIdRepository<Entity> : IBaseIdRepository<Entity>
        where Entity : BaseIdEntity, new()
    {
        private readonly DataContext _context;

        public BaseIdRepository(DataContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Entity>> GetAll()
        {
            return await _context.Set<Entity>().ToListAsync();
        }

        public virtual async Task<Entity> Get(long id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }

        public virtual async void Create(Entity item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
        }

        public virtual async void Update(Entity item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public virtual async void Delete(long id)
        {
            var item = await _context.Set<Entity>().FindAsync(id);
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
