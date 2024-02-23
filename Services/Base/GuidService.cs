using AutoMapper;
using Marketplace.Enitities.Base;
using Marketplace.Responses.Base;
using Repositories.Base;

namespace Marketplace.Services.Base
{
    public abstract class GuidService<ViewModel, Entity> : IGuidService<ViewModel>
        where ViewModel : GuidViewModel, new()
        where Entity : GuidEntity, new()
    {
        private readonly IMapper _mapper;
        private readonly IGuidRepository<Entity> _repository;
        protected GuidService(IMapper mapper, IGuidRepository<Entity> repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository;
        }

        public async Task Create(GuidViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            await _repository.Create(entity);
        }
        public void Delete(GuidViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            _repository.Delete(entity);
        }
        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }
        public void Edit(GuidViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            _repository.Edit(entity);
        }

    }
}
