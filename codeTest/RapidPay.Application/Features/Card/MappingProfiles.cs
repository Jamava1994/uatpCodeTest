using AutoMapper;
using RapidPay.Application.Features.Card.Create;
using RapidPay.Application.Features.Card.Get;
using RapidPay.Application.Features.Card.Pay;

namespace RapidPay.Application.Features.Card
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Domain.Card, GetCardQueryResponse>();

            CreateMap<Domain.Card, CreateCardCommandResponse>()
                .ForMember(dest => dest.CardNumber, y => y.MapFrom(origin => origin.Number));

            CreateMap<Domain.Payment, MakePaymentCommandResponse>()
                .ForMember(dest => dest.Card, y => y.MapFrom(origin => origin.Card));

            /* 
             * I could use "origin.Card.Number" instead of the whole object, I preferred this approach because it's more intuitive to see whether the project
             * is working properly or not. 
            */

            //.ForMember(dest => dest.CardNumber, y => y.MapFrom(origin => origin.Card.Number));
        }
    }
}
