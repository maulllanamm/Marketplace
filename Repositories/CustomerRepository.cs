using Marketplace.Enitities;
using Marketplace.Repositories.Base;

namespace Marketplace.Repositories
{
    public class CustomerRepository : BaseGuidRepository<CustomerEntity>
    {
        public CustomerRepository(DataContext context) : base(context)
        {
        }

        //public async Task<List<UserEntity>> GetAll()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        //public async Task<UserEntity> Get(Guid id)
        //{
        //    return await _context.Users.FindAsync(id);
        //}

        //public async Task<UserEntity> Create(UserEntity user)
        //{
        //    user.id = Guid.NewGuid();
        //    _context.Add(user);
        //    await _context.SaveChangesAsync();
        //    return user;
        //}

        //public async Task CreateBulk(List<UserEntity> users)
        //{
        //    await _context.AddRangeAsync(users);
        //    await _context.BulkSaveChangesAsync();
        //}

        //public async Task<UserEntity> Update(UserEntity user)
        //{
        //    _context.Update(user);
        //    await _context.SaveChangesAsync();
        //    return user;
        //}

        //public async Task<Guid> Delete(Guid id)
        //{
        //    var User = await _context.Users.FindAsync(id);
        //    _context.Remove(User);
        //    await _context.SaveChangesAsync();
        //    return User.id;
        //}

    }
}
