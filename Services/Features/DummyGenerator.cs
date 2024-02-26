using Bogus;
using Marketplace.Responses.Base;
using Marketplace.Services.Interface;

namespace Marketplace.Requests
{
    public class DummyGenerator<VM> : IDummyGenerator<VM>
        where VM : ViewModel, new()
    {
        public DummyGenerator()
        {
        }

        public List<VM> Generate(int amount, Faker<VM> faker)
        {
            return faker.Generate(amount);
        }
    }
}
