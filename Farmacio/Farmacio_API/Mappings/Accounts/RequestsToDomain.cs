using AutoMapper;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.Accounts
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateUserRequest, User>()
                .IncludeAllDerived();
            CreateMap<UpdateUserRequest, User>()
                .IncludeAllDerived();

            AddPatientMapping();
            AddPharmacistMapping();
        }

        private void AddPatientMapping()
        {
            CreateMap<CreatePatientUserRequest, Patient>();

            CreateMap<CreatePatientRequest, Account>()
                .ForMember(dst => dst.Username, opts => opts.MapFrom(src => src.Account.Username))
                .ForMember(dst => dst.Password, opts => opts.MapFrom(src => src.Account.Password))
                .ForMember(dst => dst.Email, opts => opts.MapFrom(src => src.Account.Email))
                .ForMember(dst => dst.Role, opts => opts.MapFrom(src => Role.Patient))
                .ForMember(dst => dst.ShouldChangePassword, opts => opts.MapFrom(src => false));
        }
        
        private void AddPharmacistMapping()
        {
            CreateMap<CreatePharmacistUserRequest, Pharmacist>();
            CreateMap<UpdatePharmacistUserRequest, Pharmacist>();

            CreateMap<CreatePharmacistRequest, Account>()
                .ForMember(dst => dst.Username, opts => opts.MapFrom(src => src.Account.Username))
                .ForMember(dst => dst.Password, opts => opts.MapFrom(src => src.Account.Password))
                .ForMember(dst => dst.Email, opts => opts.MapFrom(src => src.Account.Email))
                .ForMember(dst => dst.Role, opts => opts.MapFrom(src => Role.Pharmacist))
                .ForMember(dst => dst.ShouldChangePassword, opts => opts.MapFrom(src => true));

            CreateMap<UpdatePharmacistRequest, Account>()
                .ForMember(dst => dst.Role, opts => opts.MapFrom(src => Role.Pharmacist))
                .ForMember(dst => dst.Id, opts => opts.MapFrom(src => src.Account.Id));
        }
    }
}