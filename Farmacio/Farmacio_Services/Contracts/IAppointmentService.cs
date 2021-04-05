﻿using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Contracts
{
    public interface IAppointmentService : ICrudService<Appointment>
    {
        IEnumerable<Appointment> ReadForDermatologistsInPharmacy(Guid pharmacyId);
        IEnumerable<Appointment> ReadForMedicalStaff(Guid medicalStaffId);
        Appointment CreateDermatologistAppointment(CreateAppointmentDTO appointment);
        Appointment MakeAppointmentWithDermatologist(MakeAppointmentWithDermatologistDTO appointment);
        IEnumerable<Appointment> SortAppointments(Guid pharmacyId, string criteria, bool isAsc);
    }
}