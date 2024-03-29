﻿using Farmacio_Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Farmacio_Repositories.Implementation
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Dermatologist> Dermatologists { get; set; }
        public DbSet<Pharmacist> Pharmacists { get; set; }
        public DbSet<PharmacyAdmin> PharmacyAdmins { get; set; }
        public DbSet<SystemAdmin> SystemAdmins { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierMedicine> SupplierMedicines { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineReplacement> MedicineReplacements { get; set; }
        public DbSet<MedicineIngredient> MedicineIngredients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ERecipe> ERecipes { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ComplaintAboutDermatologist> ComplaintAboutDermatologists { get; set; }
        public DbSet<ComplaintAboutPharmacist> ComplaintAboutPharmacists { get; set; }
        public DbSet<ComplaintAboutPharmacy> ComplaintAboutPharmacies { get; set; }
        public DbSet<ComplaintAnswer> ComplaintAnswers { get; set; }
        public DbSet<LoyaltyPoints> LoyaltyPoints { get; set; }
        public DbSet<MedicinePoints> MedicinePoints { get; set; }

        public DbSet<AbsenceRequest> AbsenceRequests { get; set; }
        public DbSet<DermatologistWorkPlace> DermatologistWorkPlaces { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<MedicalStaffGrade> MedicalStaffGrades { get; set; }
        public DbSet<MedicineGrade> MedicineGrades { get; set; }
        public DbSet<PharmacyGrade> PharmacyGrades { get; set; }
        public DbSet<PatientPharmacyFollow> PatientPharmacyFollows { get; set; }
        public DbSet<PatientAllergy> PatientAllergies { get; set; }
        public DbSet<PharmacyMedicine> PharmacyMedicines { get; set; }
        public DbSet<MedicinePrice> MedicinePrices { get; set; }
        public DbSet<PharmacyPriceList> PharmacyPriceLists { get; set; }
        public DbSet<PharmacyOrder> PharmacyOrders { get; set; }
        public DbSet<SupplierOffer> SupplierOffers { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<NotInStock> NotInStocks { get; set; }

        public bool IsTransactionOpened { get; set; } = false;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
    }
}