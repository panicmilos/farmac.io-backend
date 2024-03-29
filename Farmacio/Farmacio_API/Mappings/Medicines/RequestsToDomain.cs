﻿using AutoMapper;
using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;

namespace Farmacio_API.Mappings.Medicines
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            AddCreateMappings();
            AddUpdateMappings();
        }

        private void AddCreateMappings()
        {
            CreateMap<CreateMedicineRequest, FullMedicineDTO>()
                .ForPath(dst => dst.Medicine.UniqueId, opts => opts.MapFrom(src => src.UniqueId))
                .ForPath(dst => dst.Medicine.Name, opts => opts.MapFrom(src => src.Name))
                .ForPath(dst => dst.Medicine.Form, opts => opts.MapFrom(src => src.Form))
                .ForPath(dst => dst.Medicine.Type, opts => opts.MapFrom(src => src.Type))
                .ForPath(dst => dst.Medicine.Manufacturer, opts => opts.MapFrom(src => src.Manufacturer))
                .ForPath(dst => dst.Medicine.IsRecipeOnly, opts => opts.MapFrom(src => src.IsRecipeOnly))
                .ForPath(dst => dst.Medicine.Contraindications, opts => opts.MapFrom(src => src.Contraindications))
                .ForPath(dst => dst.Medicine.AdditionalInfo, opts => opts.MapFrom(src => src.AdditionalInfo))
                .ForPath(dst => dst.Medicine.RecommendedDose, opts => opts.MapFrom(src => src.RecommendedDose))
                .ForMember(dst => dst.Ingredients, opts => opts.MapFrom(src => src.MedicineIngredients));

            CreateMap<CreateMedicineTypeRequest, MedicineType>();

            CreateMap<CreateMedicineIngridientRequest, MedicineIngredient>();

            CreateMap<CreateMedicineReplacementRequest, MedicineReplacement>();
        }

        private void AddUpdateMappings()
        {
            CreateMap<UpdateMedicineRequest, FullMedicineDTO>()
                .ForPath(dst => dst.Medicine.Id, opts => opts.MapFrom(src => src.Id))
                .ForPath(dst => dst.Medicine.Name, opts => opts.MapFrom(src => src.Name))
                .ForPath(dst => dst.Medicine.Form, opts => opts.MapFrom(src => src.Form))
                .ForPath(dst => dst.Medicine.Type, opts => opts.MapFrom(src => src.Type))
                .ForPath(dst => dst.Medicine.Manufacturer, opts => opts.MapFrom(src => src.Manufacturer))
                .ForPath(dst => dst.Medicine.IsRecipeOnly, opts => opts.MapFrom(src => src.IsRecipeOnly))
                .ForPath(dst => dst.Medicine.Contraindications, opts => opts.MapFrom(src => src.Contraindications))
                .ForPath(dst => dst.Medicine.AdditionalInfo, opts => opts.MapFrom(src => src.AdditionalInfo))
                .ForPath(dst => dst.Medicine.RecommendedDose, opts => opts.MapFrom(src => src.RecommendedDose))
                .ForMember(dst => dst.Ingredients, opts => opts.MapFrom(src => src.MedicineIngredients));

            CreateMap<UpdateMedicineTypeRequest, MedicineType>();

            CreateMap<UpdateMedicineIngredientRequest, MedicineIngredient>();

            CreateMap<UpdateMedicineReplacementRequest, MedicineReplacement>();
        }
    }
}