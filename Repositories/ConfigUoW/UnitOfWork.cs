using Entities.Base;
using Marketplace.Enitities.Base;
using Marketplace.Repositories;
using Marketplace.Repositories.Base;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Repositories.Base;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ConfigUoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private Dictionary<Type, object> _repositories;
        private IDbContextTransaction _transaction;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IRepository<Entity> GetRepository<Entity>() where Entity : class, IEntity
        {
            if (_repositories == null)
                _repositories = new Dictionary<Type, object>();

            var type = typeof(Entity);
            if (!_repositories.ContainsKey(type))
                _repositories[type] = new Repository<Entity>(_context);

            return (IRepository<Entity>)_repositories[type];
        }

        public IGuidRepository<GuidEntity> GetGuidRepository<GuidEntity>() where GuidEntity : class, IGuidEntity
        {
            if (_repositories == null)
                _repositories = new Dictionary<Type, object>();

            var type = typeof(GuidEntity);
            if (!_repositories.ContainsKey(type))
                _repositories[type] = new GuidRepository<GuidEntity>(_context);

            return (IGuidRepository<GuidEntity>)_repositories[type];
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.SaveChanges();
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
