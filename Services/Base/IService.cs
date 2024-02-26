using Marketplace.Responses.Base;

namespace Marketplace.Services.Base
{
    public interface IService<ViewModel>
        where ViewModel : Responses.Base.ViewModel, new()
    {
        void Create(Responses.Base.ViewModel viewModel);
        void Delete(Responses.Base.ViewModel viewModel);
        void Delete(int id);
        void Edit(Responses.Base.ViewModel viewModel);
    }
}
