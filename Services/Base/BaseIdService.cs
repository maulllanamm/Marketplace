using AutoMapper;
using Marketplace.Repositories.Base;
using Marketplace.Responses.Base;
using Repositories.Base;
using Sqids;

namespace Marketplace.Services.Base
{
    public abstract class BaseIdService<ViewModel, Entity> : IBaseIdService<ViewModel>
        where ViewModel : BaseIdViewModel, new()
        where Entity : class
    {
        private readonly IMapper _mapper;
        private readonly string _alphabet = "YnWJpoqIUMl0ceCWcKOnyEmkLs6DUAeP";
        private readonly SqidsEncoder _squidsEncoder;
        private readonly IBaseIdRepository<Entity> _repository;

        protected BaseIdService(IMapper mapper, SqidsEncoder squidsEncoder, IBaseIdRepository<Entity> repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            // Initialize SqidsEncoder with the provided alphabet
            _squidsEncoder = new SqidsEncoder(new()
            {
                Alphabet = _alphabet,
            });
            _repository = repository;
        }

        public virtual async Task<List<ViewModel>> GetAll()
        {
            var res = await _repository.GetAll();
            return _mapper.Map<List<ViewModel>>(res);

            //var encodedIds = entities.Select(x => _squidsEncoder.Encode((int)x.id)).ToList();
            //return _mapper.Map<List<ViewModel>>(encodedIds); // Menggunakan _mapper untuk memetakan antara Entity dan Res
        }

        public virtual async Task<ViewModel> Get(long id)
        {
            var res = await _repository.Get(id);
            return _mapper.Map<ViewModel>(res);

            //BaseIdRepository<Entity> repo = new BaseIdRepository<Entity>(_context);
            //var idDecode = _squidsEncoder.Decode(id);
            //Entity entitie = await repo.Get(idDecode.First());
            //var entitieRes = _mapper.Map<ViewModel>(entitie);
            //entitieRes.Id = _squidsEncoder.Encode((int)entitie.id);
            //return entitieRes;
        }

        public virtual void Create(ViewModel req)
        {
            var map = _mapper.Map<Entity>(req);
            _repository.Create(map);

            //BaseIdRepository<Entity> repo = new BaseIdRepository<Entity>(_context);
            //var entitie = _mapper.Map<Entity>(req);
            //var result = await repo.Create(entitie);
            //var entitieRes = _mapper.Map<ViewModel>(result);
            //entitieRes.Id = _squidsEncoder.Encode((int)entitie.id);
            //return entitieRes;
        }

        public virtual void Update(ViewModel req)
        {
            var map = _mapper.Map<Entity>(req);
            _repository.Update(map);

            //BaseIdRepository<Entity> repo = new BaseIdRepository<Entity>(_context);
            //var idDecode = _squidsEncoder.Decode(req.Id);
            //Entity data = await repo.Get(idDecode.First());
            //if (data != null)
            //{
            //    // Menggunakan AutoMapper untuk memetakan properti dari req ke data
            //    _mapper.Map(req, data);

            //    var updated = await _repository.Update(data);
            //    var entitieRes = _mapper.Map<ViewModel>(updated);
            //    entitieRes.Id = _squidsEncoder.Encode((int)updated.id);
            //    return entitieRes;
            //}
            //return new ViewModel();
        }

        public virtual void Delete(long id)
        {
            _repository.Delete(id);
            //BaseIdRepository<Entity> repo = new BaseIdRepository<Entity>(_context);
            //var idDecode = _squidsEncoder.Decode(id);
            //var res = await repo.Delete(idDecode.First());
            //return id;
        }
    }
}
