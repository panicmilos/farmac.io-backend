using System;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Farmacio_Repositories.Implementation
{
    public class DermatologistWorkPlacesRepository : Repository<DermatologistWorkPlace>, IDermatologistWorkPlacesRepository
    {
        public DermatologistWorkPlacesRepository(DatabaseContext context) :
            base(context)
        {
        }

        public IEnumerable<DermatologistWorkPlace> ReadForDermatologist(Guid dermatologistId)
        {
            return _context.DermatologistWorkPlaces
                .FromSqlRaw(
                    $"SELECT * FROM DermatologistWorkPlaces WHERE DermatologistId = \"{dermatologistId}\" FOR UPDATE;")
                .ToList();
        }

        public DermatologistWorkPlace ReadForDermatologistInPharmacy(Guid dermatologistId, Guid pharmacyId)
        {
            return _context.DermatologistWorkPlaces
                .FromSqlRaw(
                    $"SELECT * FROM DermatologistWorkPlaces WHERE DermatologistId = \"{dermatologistId}\" AND PharmacyId = \"{pharmacyId}\" FOR UPDATE")
                .FirstOrDefault();
        }
    }
}