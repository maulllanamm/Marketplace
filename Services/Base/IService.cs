using Marketplace.Responses;
using Marketplace.Responses.Base;

namespace Marketplace.Services.Base
{
    public interface IService<ViewModel>
        where ViewModel : Responses.Base.ViewModel, new()
    {
        public Task<List<ViewModel>> GetAll();
        public Task<List<ViewModel>> GetAll(int page);
        public Task<ViewModel> GetById(int id);
        public Task<List<ViewModel>> GetByListId(List<int> listId);
        public Task<List<ViewModel>> GetByListProperty(string field, string[] values);
        public Task<ViewModel> Create(ViewModel viewModel);
        public Task<string> CreateBulk(List<ViewModel> viewModels);
        public Task<ViewModel> Update(ViewModel viewModel);
        public Task<string> UpdateBulk(List<ViewModel> viewModels);
        public Task<int> Delete(int id);
        public Task<string> DeleteBulk(List<ViewModel> viewModels);
        public Task<int> SoftDelete(int id);
        public Task<string> SoftDeleteBulk(List<ViewModel> viewModels);
        public Task<int> Count();
    }
}
