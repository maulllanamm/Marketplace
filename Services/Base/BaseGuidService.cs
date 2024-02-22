using AutoMapper;
using Marketplace.Responses.Base;
using Repositories.Base;

namespace Marketplace.Services.Base
{
    public abstract class BaseGuidService<ViewModel,Entity> : IBaseGuidService<ViewModel>
        where ViewModel : BaseGuidViewModel, new()
        where Entity : class
    {
        private readonly IMapper _mapper;
        private readonly IBaseGuidRepository<Entity> _repository;
        protected BaseGuidService(IMapper mapper, IBaseGuidRepository<Entity> repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository;
        }

        public virtual async Task<List<ViewModel>> GetAll()
        {
            var res = await _repository.GetAll();
            return _mapper.Map<List<ViewModel>>(res);

            //var encodedIds = entities.Select(x => _squidsEncoder.Encode((int)x.id)).ToList();
            //return _mapper.Map<List<ViewModel>>(encodedIds); // Menggunakan _mapper untuk memetakan antara Entity dan Res
        }

        public virtual async Task<ViewModel> Get(Guid id)
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

        public virtual void Delete(Guid id)
        {
            _repository.Delete(id);
            //BaseIdRepository<Entity> repo = new BaseIdRepository<Entity>(_context);
            //var idDecode = _squidsEncoder.Decode(id);
            //var res = await repo.Delete(idDecode.First());
            //return id;
        }
    }
}
