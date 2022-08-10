using AutoMapper;
using RapidPay.Application.Features.User.Authenticate;
using RapidPay.Application.Features.User.Create;

namespace RapidPay.Application.Features.User
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateUserCommand, Domain.User>().ReverseMap();
            CreateMap<SignInCommand, Domain.User>();
        }
    }
}
