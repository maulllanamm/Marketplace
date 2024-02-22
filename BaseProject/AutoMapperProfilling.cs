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
            CreateMap<CustomerViewModel, CustomerEntity>().EqualityComparison((odto, o) => odto.Id == o.id).ReverseMap();
            CreateMap<CustomerEntity, CustomerViewModel>().EqualityComparison((odto, o) => odto.id == o.Id).ReverseMap();
        }
    }
}
