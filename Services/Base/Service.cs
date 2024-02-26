using AutoMapper;
using Marketplace.Repositories.Base;
using Marketplace.Responses.Base;
using Repositories.Base;
using Sqids;

namespace Marketplace.Services.Base
{
    public abstract class Service<ViewModel, Entity> : IService<ViewModel>
        where ViewModel : Responses.Base.ViewModel, new()
        where Entity : class
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Entity> _repository;

        protected Service(IMapper mapper, IRepository<Entity> repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository;
        }

        public void Create(Responses.Base.ViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            _repository.Create(entity);
        }
        public void Delete(Responses.Base.ViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            _repository.Delete(entity);
        }
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
        public void Edit(Responses.Base.ViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            _repository.Edit(entity);
        }
    }
}
