using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Marketplace.Enitities;
using Marketplace.Enitities.Base;
using Marketplace.Requests;
using Marketplace.Responses;
using Marketplace.Responses.Base;

namespace Marketplace
{
    public class AutoMapperProfilling : Profile
    {
        public AutoMapperProfilling()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            CreateMap<UserViewModel, User>().EqualityComparison((odto, o) => odto.Id == o.id).ReverseMap();
            CreateMap<User, UserViewModel>().EqualityComparison((odto, o) => odto.id == o.Id).ReverseMap();

            CreateMap<ProductViewModel, Product>().EqualityComparison((odto, o) => odto.Id == o.id).ReverseMap();
            CreateMap<Product, ProductViewModel>().EqualityComparison((odto, o) => odto.id == o.Id).ReverseMap();

            CreateMap<RoleViewModel, Role>().EqualityComparison((odto, o) => odto.Id == o.id).ReverseMap();
            CreateMap<Role, RoleViewModel>().EqualityComparison((odto, o) => odto.id == o.Id).ReverseMap();

            CreateMap<PermissionViewModel, Permission>().EqualityComparison((odto, o) => odto.Id == o.id).ReverseMap();
            CreateMap<Permission, PermissionViewModel>().EqualityComparison((odto, o) => odto.id == o.Id).ReverseMap();

        }
    }
}
