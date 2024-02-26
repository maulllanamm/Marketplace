using Marketplace.Responses.Base;
using System.Linq.Expressions;

namespace Marketplace.Services.Base
{
    public interface IGuidService<ViewModel>
        where ViewModel : GuidViewModel, new()
    {
        public Task<List<ViewModel>> GetAll();
        public Task<ViewModel> GetById(Guid id);
        public Task<ViewModel> Create(ViewModel viewModel);
        public Task<string> CreateBulk(List<ViewModel> viewModels);
        public Task<ViewModel> Update(ViewModel viewModel);
        public Task<string> UpdateBulk(List<ViewModel> viewModels);
        public Task<Guid> Delete(Guid id);
        public Task<string> DeleteBulk(List<ViewModel> viewModels);
        public Task<Guid> SoftDelete(Guid id);
        public Task<string> SoftDeleteBulk(List<ViewModel> viewModels);
    }
}
