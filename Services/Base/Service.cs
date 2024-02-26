using AutoMapper;
using Repositories.Base;
using System.Diagnostics;

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
        public async Task<List<ViewModel>> GetAll()
        {
            var entities = await _repository.GetAll();
            return _mapper.Map<List<ViewModel>>(entities);
        }
        public async Task<ViewModel> GetById(int id)
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
        public async Task<int> Delete(int id)
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

        public async Task<int> SoftDelete(int id)
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
