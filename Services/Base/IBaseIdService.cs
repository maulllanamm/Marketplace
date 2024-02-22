using Marketplace.Responses.Base;

namespace Marketplace.Services.Base
{
    public interface IBaseIdService<ViewModel>
        where ViewModel : BaseIdViewModel, new()
    {
        Task<List<ViewModel>> GetAll();
        Task<ViewModel> Get(long id);
        void Create(ViewModel viewModel);
        void Update(ViewModel viewModel);
        void Delete(long id);
    }
}
