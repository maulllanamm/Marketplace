using AutoMapper;
using Marketplace.Enitities.Base;
using Marketplace.Responses.Base;
using Repositories.Base;
using System.Diagnostics;

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

        public async Task<List<ViewModel>> GetAll()
        {
            var entities = await _repository.GetAll();
            return _mapper.Map<List<ViewModel>>(entities);
        }
        public async Task<ViewModel> GetById(Guid id)
        {
            var entity = await _repository.GetById(id);
            return _mapper.Map<ViewModel>(entity);
        }
        public async Task<ViewModel> Create(ViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            var res = await _repository.Create(entity);
            return _mapper.Map<ViewModel>(res);
        }
        public async Task<string> CreateBulk(List<ViewModel> viewModels)
        {
            var entities = _mapper.Map<List<Entity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.CreateBulk(entities);
            stopwatch.Stop();
            return $"Success create: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }

        public async Task<ViewModel> Update(ViewModel viewModel)
        {
            var entity = _mapper.Map<Entity>(viewModel);
            var res = await _repository.Update(entity);
            return _mapper.Map<ViewModel>(res);
        }

        public async Task<string> UpdateBulk(List<ViewModel> viewModels)
        {
            var entities = _mapper.Map<List<Entity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.UpdateBulk(entities);
            stopwatch.Stop();
            return $"Success update: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }
        public async Task<Guid> Delete(Guid id)
        {
            return await _repository.Delete(id);
        }
        public async Task<string> DeleteBulk(List<ViewModel> viewModels)
        {
            var entities = _mapper.Map<List<Entity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.DeleteBulk(entities);
            stopwatch.Stop();
            return $"Success delete: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }

        public async Task<Guid> SoftDelete(Guid id)
        {
            return await _repository.SoftDelete(id);
        }
        public async Task<string> SoftDeleteBulk(List<ViewModel> viewModels)
        {
            var entities = _mapper.Map<List<Entity>>(viewModels);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = await _repository.SoftDeleteBulk(entities);
            stopwatch.Stop();
            return $"Success delete: {res} data , Elapsed time: {stopwatch.Elapsed}";
        }

    }
}
