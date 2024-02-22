using Marketplace.Responses.Base;

namespace Marketplace.Services.Base
{
    public interface IBaseGuidService<ViewModel>
        where ViewModel : BaseGuidViewModel, new()
    {
        Task<List<ViewModel>> GetAll();
        Task<ViewModel> Get(Guid id);
        void Create(ViewModel viewModel);
        void Update(ViewModel viewModel);
        void Delete(Guid id);
    }
}
