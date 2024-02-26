using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Marketplace.Enitities;
using Marketplace.Requests;
using Marketplace.Responses;

namespace Marketplace
{
    public class AutoMapperProfilling : Profile
    {
        public AutoMapperProfilling()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            CreateMap<CustomerViewModel, Customer>().EqualityComparison((odto, o) => odto.Id == o.id).ReverseMap();
            CreateMap<Customer, CustomerViewModel>().EqualityComparison((odto, o) => odto.id == o.Id).ReverseMap();

            CreateMap<ProductViewModel, Product>().EqualityComparison((odto, o) => odto.Id == o.id).ReverseMap();
            CreateMap<Product, ProductViewModel>().EqualityComparison((odto, o) => odto.id == o.Id).ReverseMap();
        }
    }
}
