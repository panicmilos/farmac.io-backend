using AutoMapper;
using Farmacio_Models.Domain;
using Farmacio_API.Contracts.Requests.Grades;

namespace Farmacio_API.Mappings.Grades
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateMedicalStaffGradeRequest, MedicalStaffGrade>()
                .ForMember(dst => dst.Value, src => src.MapFrom(src => src.Grade))
                .ForMember(dst => dst.MedicalStaffId, src => src.MapFrom(src => src.MedicalStaffId));

            CreateMap<CreateMedicineGradeRequest, MedicineGrade>();
            CreateMap<CreatePharmacyGradeRequest, PharmacyGrade>();
        }
    }
}