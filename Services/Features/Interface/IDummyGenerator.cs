using Bogus;
using Marketplace.Responses.Base;

namespace Marketplace.Services.Interface
{
    public interface IDummyGenerator<VM> 
        where VM : ViewModel, new()
    {
        public List<VM> Generate(int amount, Faker<VM> faker);
    }
}
