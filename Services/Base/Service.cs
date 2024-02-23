using AutoMapper;
using Marketplace.Repositories.Base;
using Marketplace.Responses.Base;
using Repositories.Base;
using Sqids;

namespace Marketplace.Services.Base
{
    public abstract class Service<ViewModel, Entity> : IService<ViewModel>
        where ViewModel : BaseViewModel, new()
        where Entity : class
    {
        private readonly IMapper _mapper;
        private readonly string _alphabet = "YnWJpoqIUMl0ceCWcKOnyEmkLs6DUAeP";
        private readonly SqidsEncoder _squidsEncoder;
        private readonly IRepository<Entity> _repository;

        protected Service(IMapper mapper, SqidsEncoder squidsEncoder, IRepository<Entity> repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            // Initialize SqidsEncoder with the provided alphabet
            _squidsEncoder = new SqidsEncoder(new()
            {
                Alphabet = _alphabet,
            });
            _repository = repository;
        }

        public void Create(BaseViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            _repository.Create(entity);
        }
        public void Delete(BaseViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            _repository.Delete(entity);
        }
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
        public void Edit(BaseViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            _repository.Edit(entity);
        }
    }
}
