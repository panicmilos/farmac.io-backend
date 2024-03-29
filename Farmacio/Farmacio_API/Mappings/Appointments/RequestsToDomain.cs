﻿using AutoMapper;
using Farmacio_API.Contracts.Requests.Appointments;
using Farmacio_Models.DTO;

namespace Farmacio_API.Mappings.Appointments
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateAppointmentRequest, CreateAppointmentDTO>();
            CreateMap<CreateAppointmentWithDermatologist, MakeAppointmentWithDermatologistDTO>();
            CreateMap<CreateReportRequest, CreateReportDTO>();
        }
    }
}