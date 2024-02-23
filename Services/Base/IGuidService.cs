using Marketplace.Responses.Base;
using System.Linq.Expressions;

namespace Marketplace.Services.Base
{
    public interface IGuidService<ViewModel>
        where ViewModel : GuidViewModel, new()
    {
        Task Create(GuidViewModel viewModel);
        void Delete(GuidViewModel viewModel);
        void Delete(Guid id);
        void Edit(GuidViewModel viewModel);
    }
}
