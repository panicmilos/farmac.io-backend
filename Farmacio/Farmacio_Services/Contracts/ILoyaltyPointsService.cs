using Farmacio_Models.Domain;
using System;

namespace Farmacio_Services.Contracts
{
    public interface ILoyaltyPointsService : ICrudService<LoyaltyPoints>
    {
        LoyaltyPoints ReadOrCreate();

        int ReadExaminationPoints();

        int ReadConsultationPoints();

        int ReadPointsFor(Guid medicineId);

        Account GivePointsFor(Reservation reservation);

        Account GivePointsFor(Appointment appointment);
    }
}