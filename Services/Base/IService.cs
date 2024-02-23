using Marketplace.Responses.Base;

namespace Marketplace.Services.Base
{
    public interface IService<ViewModel>
        where ViewModel : BaseViewModel, new()
    {
        void Create(BaseViewModel viewModel);
        void Delete(BaseViewModel viewModel);
        void Delete(int id);
        void Edit(BaseViewModel viewModel);
    }
}
