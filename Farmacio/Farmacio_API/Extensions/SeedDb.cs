using Farmacio_Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

public static class SeedDb
{
    public static IHost SeedDbContext<TContext>(this IHost host) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetService<TContext>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            logger.LogInformation($"Seeding database associated with context {typeof(TContext).Name}");
            Seed(context);
            logger.LogInformation($"Finished seeding database associated with context {typeof(TContext).Name}");
        }

        return host;
    }

    private static void AddIFNotDuplicate<T>(DbContext context, T entity) where T : BaseEntity
    {
        if (context.Find<T>(entity.Id) == null)
        {
            context.Add(entity);
        }
    }

    private static void Seed<TContext>(TContext context) where TContext : DbContext
    {
        var address1 = new Address
        {
            StreetName = "Jevrejska",
            StreetNumber = "14a",
            City = "Novi Sad",
            State = "Srbija",
            Lat = 45.254410f,
            Lng = 19.842550f
        };

        var address2 = new Address
        {
            StreetName = "Zmaj Jovina",
            StreetNumber = "27a",
            City = "Novi Sad",
            State = "Srbija",
            Lat = 45.254410f,
            Lng = 19.842550f
        };

        var address3 = new Address
        {
            StreetName = "Bulevar Oslobodjenja",
            StreetNumber = "71a",
            City = "Novi Sad",
            State = "Srbija",
            Lat = 45.254410f,
            Lng = 19.842550f
        };

        AddIFNotDuplicate(context, address1);
        AddIFNotDuplicate(context, address2);
        AddIFNotDuplicate(context, address3);

        var medicineType1 = new MedicineType
        {
            TypeName = "Analgetik"
        };

        AddIFNotDuplicate(context, medicineType1);

        var medicineIngridient1 = new MedicineIngredient
        {
            Name = "kroskarmeloza-natrijum",
            MassInMilligrams = 1
        };

        var medicineIngridient2 = new MedicineIngredient
        {
            Name = "paracetamol",
            MassInMilligrams = 500
        };

        var ingredients1 = new List<MedicineIngredient> { medicineIngridient1, medicineIngridient2 };

        var medicineIngridient3 = new MedicineIngredient
        {
            Name = "ibuprofena",
            MassInMilligrams = 400
        };

        var medicineIngridient4 = new MedicineIngredient
        {
            Name = "laktoza",
            MassInMilligrams = 10
        };

        var ingredients2 = new List<MedicineIngredient>
        {
            medicineIngridient1,
            medicineIngridient3,
            medicineIngridient4
        };

        var ingredients3 = new List<MedicineIngredient>
        {
            medicineIngridient2,
            medicineIngridient3,
            medicineIngridient4
        };

        AddIFNotDuplicate(context, medicineIngridient1);
        AddIFNotDuplicate(context, medicineIngridient2);
        AddIFNotDuplicate(context, medicineIngridient3);
        AddIFNotDuplicate(context, medicineIngridient4);

        var medicine1 = new Medicine
        {
            UniqueId = "123abc",
            Name = "Paracetamol",
            Form = MedicineForm.Tablet,
            Type = medicineType1,
            Manufacturer = "Hemofarm",
            IsRecipeOnly = false,
            Contraindications = "Ko�ni osip, alergijse reakcije, povi�en krvni pritisak, glavobolju, vrtoglavicu,stoma�ne tegobe(mu�nina ili proliv), " +
            "probleme sa spavanjem, lupanje srca i poreme�aje krvitrombocitopenija i agranulocitoza(smanjenje broja krvnih plo�ica i belih krvnih " +
            "zrnaca koji se javljaj kod �este i dugotrajne upotrebe)",
            AdditionalInfo = "Izgled tableta: okrugle tablete, bele boje, koje sa jedne strane imaju utisnutu podeonu crtu. ",
            RecommendedDose = "2 do 4 tablete dnevno",
            AverageGrade = 0,
        };

        var medicine2 = new Medicine
        {
            Id = new Guid("08d91521-5bf4-4a5e-8740-d68fcde43c58"),
            UniqueId = "321cba",
            Name = "Brufen",
            Form = MedicineForm.Tablet,
            Type = medicineType1,
            Manufacturer = "Galenika",
            IsRecipeOnly = false,
            Contraindications = "Glavobolja, vrtoglavica, gastrointestinalna ne�eljena dejstva (lo�e varenje, dijareja, mu�nina, povra�anje, bol u stomaku, " +
            "nadimanje, konstipacija, crna stolica, krvarenje u stomaku i crevima, povra�anje krvi), osip, zamor",
            AdditionalInfo = "Izgled: bela, ovalna, bikonveksna film tableta",
            RecommendedDose = "Uobi�ajena doza je 600-1800 mg na dan podeljena u nekoliko doza",
            AverageGrade = 0,
        };

        var medicine3 = new Medicine
        {
            Id = new Guid("08d91521-5c05-422b-8f14-df14e1ee1016"),
            UniqueId = "321cba",
            Name = "Caffetin",
            Form = MedicineForm.Tablet,
            Type = medicineType1,
            Manufacturer = "Hemofarm",
            IsRecipeOnly = true,
            Contraindications = "Gadjenje, povracanje, nesanica, lupanje srca ili ubrzan rad srca, porecemaji funkcije jetre i bubrega, zavisnost.",
            AdditionalInfo = "Izgled: bela, ovalna, ravna.",
            RecommendedDose = "3-4 tablete dnevno",
            AverageGrade = 0,
        };

        AddIFNotDuplicate(context, medicine1);
        AddIFNotDuplicate(context, medicine2);
        AddIFNotDuplicate(context, medicine3);

        var medicinePrice1 = new MedicinePrice
        {
            MedicineId = medicine1.Id,
            Price = 200,
            ActiveFrom = DateTime.Now
        };

        var medicinePrice2 = new MedicinePrice
        {
            MedicineId = medicine2.Id,
            Price = 300,
            ActiveFrom = DateTime.Now
        };

        var medicinePrices1 = new List<MedicinePrice> { medicinePrice1, medicinePrice2 };

        AddIFNotDuplicate(context, medicinePrice1);
        AddIFNotDuplicate(context, medicinePrice2);

        var medicinePrice3 = new MedicinePrice
        {
            MedicineId = medicine1.Id,
            Price = 235,
            ActiveFrom = DateTime.Now
        };

        var medicinePrice4 = new MedicinePrice
        {
            MedicineId = medicine3.Id,
            Price = 350,
            ActiveFrom = DateTime.Now
        };

        var medicinePrices2 = new List<MedicinePrice> { medicinePrice3, medicinePrice4 };

        AddIFNotDuplicate(context, medicinePrice3);
        AddIFNotDuplicate(context, medicinePrice4);

        var medicinePrice5 = new MedicinePrice
        {
            MedicineId = medicine2.Id,
            Price = 290,
            ActiveFrom = DateTime.Now
        };

        var medicinePrice6 = new MedicinePrice
        {
            MedicineId = medicine3.Id,
            Price = 375,
            ActiveFrom = DateTime.Now
        };

        var medicinePrices3 = new List<MedicinePrice> { medicinePrice5, medicinePrice6 };

        AddIFNotDuplicate(context, medicinePrice5);
        AddIFNotDuplicate(context, medicinePrice6);

        var pharmacy1 = new Pharmacy
        {
            Id = new Guid("08d91521-5c7d-4f06-85b3-85ce3c1ad6a3"),
            Name = "BENU Apoteka",
            Address = address1,
            Description = "Apotekarska ustanova BENU je najveci lanac apoteka u Srbiji i deo je velike medjunarodne kompanije PHOENIX iz Nemacke.",
            AverageGrade = 0,
        };

        var pharmacy2 = new Pharmacy
        {
            Name = "Viva Farm",
            Address = address2,
            Description = "Tu smo da zajedno sa vama uti�emo na o�uvanje dobog zdravlja i spre�avanje razvoja bolesti za koje postoji rizik ili predispozicija.",
            AverageGrade = 0,
        };

        var pharmacy3 = new Pharmacy
        {
            Id = new Guid("08d91521-5ca1-4f12-841c-270f430cde13"),
            Name = "Dr.Max",
            Address = address3,
            Description = "Dr.Max je me�unarodni lanac apoteka, koji je prisutan u 6 " +
                          "zemalja Centralno Isto�ne Evrope sa preko 2000 apoteka i 12000 zaposlenih. Od 2017. godine, Dr.Max je prisutan i na tr�i�tu Srbije.",
            AverageGrade = 0,
        };

        AddIFNotDuplicate(context, pharmacy1);
        AddIFNotDuplicate(context, pharmacy2);
        AddIFNotDuplicate(context, pharmacy3);

        var pharmacyMedicine11 = new PharmacyMedicine
        {
            PharmacyId = pharmacy1.Id,
            MedicineId = medicine1.Id,
            Quantity = 4
        };

        var pharmacyMedicine12 = new PharmacyMedicine
        {
            PharmacyId = pharmacy1.Id,
            MedicineId = medicine2.Id,
            Quantity = 5
        };

        var pharmacyMedicine21 = new PharmacyMedicine
        {
            PharmacyId = pharmacy2.Id,
            MedicineId = medicine1.Id,
            Quantity = 2
        };

        var pharmacyMedicine22 = new PharmacyMedicine
        {
            PharmacyId = pharmacy2.Id,
            MedicineId = medicine3.Id,
            Quantity = 1
        };

        var pharmacyMedicine31 = new PharmacyMedicine
        {
            PharmacyId = pharmacy3.Id,
            MedicineId = medicine2.Id,
            Quantity = 2
        };

        var pharmacyMedicine32 = new PharmacyMedicine
        {
            PharmacyId = pharmacy3.Id,
            MedicineId = medicine3.Id,
            Quantity = 7
        };

        AddIFNotDuplicate(context, pharmacyMedicine11);
        AddIFNotDuplicate(context, pharmacyMedicine12);
        AddIFNotDuplicate(context, pharmacyMedicine21);
        AddIFNotDuplicate(context, pharmacyMedicine22);
        AddIFNotDuplicate(context, pharmacyMedicine31);
        AddIFNotDuplicate(context, pharmacyMedicine32);

        var priceList1 = new PharmacyPriceList
        {
            PharmacyId = pharmacy1.Id,
            ExaminationPrice = 1500f,
            ConsultationPrice = 800f,
            MedicinePriceList = medicinePrices1
        };

        var priceList2 = new PharmacyPriceList
        {
            PharmacyId = pharmacy2.Id,
            ExaminationPrice = 1200f,
            ConsultationPrice = 600f,
            MedicinePriceList = medicinePrices2
        };

        var priceList3 = new PharmacyPriceList
        {
            PharmacyId = pharmacy3.Id,
            ExaminationPrice = 1300f,
            ConsultationPrice = 750f,
            MedicinePriceList = medicinePrices3
        };

        AddIFNotDuplicate(context, priceList1);
        AddIFNotDuplicate(context, priceList2);
        AddIFNotDuplicate(context, priceList3);

        var loyaltyProgram1 = new LoyaltyProgram
        {
            Discount = 2,
            MinPoints = 50,
            Name = "Regular"
        };

        var loyaltyProgram2 = new LoyaltyProgram
        {
            Discount = 5,
            MinPoints = 150,
            Name = "Silver"
        };

        var loyaltyProgram3 = new LoyaltyProgram
        {
            Discount = 10,
            MinPoints = 500,
            Name = "Gold"
        };

        AddIFNotDuplicate(context, loyaltyProgram1);
        AddIFNotDuplicate(context, loyaltyProgram2);
        AddIFNotDuplicate(context, loyaltyProgram3);

        var patient1 = new Patient
        {
            Id = new Guid("08d91521-5da2-43ab-8c3b-c16c102f0848"),
            FirstName = "Pera",
            LastName = "Peric",
            DateOfBirth = DateTime.Now,
            PID = "1234567891234",
            PhoneNumber = "06456987412",
            Address = address1,
            Points = 0,
            NegativePoints = 0,
            LoyaltyProgram = loyaltyProgram1
        };

        var allergy1 = new PatientAllergy
        {
            MedicineId = medicine1.Id,
            PatientId = patient1.Id
        };

        AddIFNotDuplicate(context, allergy1);

        var account1 = new Account
        {
            Username = "pera",
            Password = "NZ72wIqfGVuYpvNIuOqSE6PrBzzhhGY73SbO7hg+/rM=",
            Salt = "rR69PlY8GTfpkoy9ztGyUA1koyk2JmZvMnKxEWvuf5k=",
            Email = "pera.peric@gmail.com",
            Role = Role.Patient,
            IsVerified = true,
            ShouldChangePassword = false,
            User = patient1
        };

        var patient2 = new Patient
        {
            FirstName = "Mikic",
            LastName = "Mikic",
            DateOfBirth = DateTime.Now,
            PID = "7894561233216",
            PhoneNumber = "06145789631",
            Address = address2,
            Points = 0,
            NegativePoints = 1,
            LoyaltyProgram = loyaltyProgram2
        };

        var account2 = new Account
        {
            Username = "mika",
            Password = "rFP3YfAQfskxjarItdztYb63agyWcD4Id7GrTiX7wGw=",
            Salt = "9PQE00Eg2wgqRj8n3QwBY0P5biEVwvl7wZuK6lbr3OI=",
            Email = "mika.mika@gmail.com",
            Role = Role.Patient,
            IsVerified = true,
            ShouldChangePassword = false,
            User = patient2
        };

        var patient3 = new Patient
        {
            Id = new Guid("08d91521-5dc6-45c3-81ec-26b64d85b5ea"),
            FirstName = "Janko",
            LastName = "Jankovic",
            DateOfBirth = DateTime.Now,
            PID = "4561239874561",
            PhoneNumber = "0654789123",
            Address = address3,
            Points = 0,
            NegativePoints = 3,
            LoyaltyProgram = loyaltyProgram3
        };

        var account3 = new Account
        {
            Username = "janko",
            Password = "qOQzfKI3MOS3tuei1enUaGDvCky9kL71xbqvbS8s0xk=",
            Salt = "fJHZyhp6wai3LWwkXrrrCgLSqMFKxnYgphKiQ55XUCE=",
            Email = "janko.jankovic@gmail.com",
            Role = Role.Patient,
            IsVerified = false,
            ShouldChangePassword = false,
            User = patient3
        };

        var systemAdmin1 = new SystemAdmin
        {
            FirstName = "Sistemko",
            LastName = "Adminic",
            DateOfBirth = DateTime.Now.AddYears(-35),
            PID = "9852356231569",
            PhoneNumber = "0641236547",
            Address = address2,
        };

        var account4 = new Account
        {
            Username = "sysadmin",
            Password = "YpvznSTblbad6/C3aRmCu5ADTXj/bChmM/SnjfKncfE=",
            Salt = "wGmHx+1wQZmnWPv6WssqtldX8KFcHRNxUy9/kTnDQXU=",
            Email = "sistem.admin@gmail.com",
            Role = Role.SystemAdmin,
            IsVerified = true,
            ShouldChangePassword = false,
            User = systemAdmin1
        };

        var pharmacyAdmin1 = new PharmacyAdmin
        {
            FirstName = "Farmaci",
            LastName = "Adminic",
            DateOfBirth = DateTime.Now.AddYears(-36),
            PID = "9842358961589",
            PhoneNumber = "0647984523",
            Address = address1,
            Pharmacy = pharmacy1
        };

        var account5 = new Account
        {
            Username = "pharmadmin",
            Password = "vjEd42+JWk7zfK2U4SlCHoFZjtm+01w+7AgbzkCnDQM=",
            Salt = "4t0GdyenPDTpi6giXSlGXEmGSfKn6xlDjnRw1UAm1KQ=",
            Email = "pharmacy1admin@gmail.com",
            Role = Role.PharmacyAdmin,
            IsVerified = true,
            ShouldChangePassword = false,
            User = pharmacyAdmin1
        };

        var pharmacist1 = new Pharmacist
        {
            FirstName = "Petar",
            LastName = "Petrovic",
            DateOfBirth = DateTime.Now.AddYears(-39),
            PID = "9822359563218",
            PhoneNumber = "0648521694",
            Address = address2,
            Pharmacy = pharmacy1,
            WorkTime = new WorkTime
            {
                From = new DateTime(2020, 1, 1, 7, 0, 0),
                To = new DateTime(2020, 1, 1, 13, 0, 0),
            },
            AverageGrade = 0,
        };

        var account6 = new Account
        {
            Username = "ppetar",
            Password = "tnQCFUMIuJ+6gwwuKraVpz3YRwPuVnAcOd9IejU2Rnc=",
            Salt = "zqblcOiRflN5u+qaXu2o77m14j5tpruNbLzMi/DRAZA=",
            Email = "petar82petrovic@gmail.com",
            Role = Role.Pharmacist,
            IsVerified = true,
            ShouldChangePassword = false,
            User = pharmacist1
        };

        var pharmacist2 = new Pharmacist
        {
            FirstName = "Jelena",
            LastName = "Petrovic",
            DateOfBirth = DateTime.Now.AddYears(-27),
            PID = "9912125626598",
            PhoneNumber = "0647512493",
            Address = address3,
            Pharmacy = pharmacy2,
            WorkTime = new WorkTime
            {
                From = new DateTime(2020, 1, 1, 8, 0, 0),
                To = new DateTime(2020, 1, 1, 14, 0, 0),
            },
            AverageGrade = 0,
        };

        var account7 = new Account
        {
            Username = "pjelena",
            Password = "7ku8vhgfafrVgVlbHCUhwgaqmqjbFj17uKn3K99Snvo=",
            Salt = "LqJzcXbgElZ4yxor4y3FYNBy++Pmgv5TTnUBOe7a++g=",
            Email = "jelpetrovic@gmail.com",
            Role = Role.Pharmacist,
            IsVerified = true,
            ShouldChangePassword = false,
            User = pharmacist2
        };

        var dermatologist1 = new Dermatologist
        {
            FirstName = "Milica",
            LastName = "Mikic",
            DateOfBirth = DateTime.Now.AddYears(-35),
            PID = "9872125346751",
            PhoneNumber = "0649467315",
            Address = address2,
            AverageGrade = 0,
        };

        var account8 = new Account
        {
            Username = "mmilica",
            Password = "CvuOUuMG2p+NtlpKoXd5udKBjzWqi2HdRxsi6LAqhTM=",
            Salt = "CvuOUuMG2p+NtlpKoXd5udKBjzWqi2HdRxsi6LAqhTM=",
            Email = "milicamikic@gmail.com",
            Role = Role.Dermatologist,
            IsVerified = true,
            ShouldChangePassword = false,
            User = dermatologist1
        };

        var dermatologist2 = new Dermatologist
        {
            FirstName = "Milos",
            LastName = "Mikic",
            DateOfBirth = DateTime.Now.AddYears(-37),
            PID = "9852346197521",
            PhoneNumber = "0649467312",
            Address = address1,
            AverageGrade = 0,
        };

        var account9 = new Account
        {
            Username = "mmilos",
            Password = "Xnxvvx5uwm/+GdB6MoHV8U6w4x4ZU775AqVpphL1eDo=",
            Salt = "xWhX60uTJunkr3IqnlD/GzVP81LChadjGbGZrmD2n4Q=",
            Email = "milosmikic85@gmail.com",
            Role = Role.Dermatologist,
            IsVerified = true,
            ShouldChangePassword = false,
            User = dermatologist2
        };

        var appointment1 = new Appointment
        {
            DateTime = DateTime.Now,
            Duration = 30,
            MedicalStaff = dermatologist1,
            IsReserved = true,
            Patient = patient1,
            Pharmacy = pharmacy2,
            Price = 1200,
        };

        var appointment2 = new Appointment
        {
            DateTime = DateTime.Now.AddDays(-1),
            Duration = 30,
            MedicalStaff = dermatologist1,
            IsReserved = true,
            Patient = patient2,
            Pharmacy = pharmacy2,
            Price = 1200,
        };

        var appointment3 = new Appointment
        {
            DateTime = DateTime.Now.AddDays(-2),
            Duration = 30,
            MedicalStaff = dermatologist1,
            IsReserved = true,
            Patient = patient3,
            Pharmacy = pharmacy2,
            Price = 1200,
        };

        var notInStock1 = new NotInStock
        {
            PharmacyId = pharmacy1.Id,
            MedicineId = medicine1.Id
        };

        var notInStock2 = new NotInStock
        {
            PharmacyId = pharmacy2.Id,
            MedicineId = medicine2.Id
        };

        var notInStock3 = new NotInStock
        {
            PharmacyId = pharmacy3.Id,
            MedicineId = medicine3.Id
        };

        AddIFNotDuplicate(context, patient1);
        AddIFNotDuplicate(context, patient2);
        AddIFNotDuplicate(context, patient3);
        AddIFNotDuplicate(context, systemAdmin1);
        AddIFNotDuplicate(context, pharmacyAdmin1);
        AddIFNotDuplicate(context, pharmacist1);
        AddIFNotDuplicate(context, pharmacist2);
        AddIFNotDuplicate(context, dermatologist1);
        AddIFNotDuplicate(context, dermatologist2);
        AddIFNotDuplicate(context, account1);
        AddIFNotDuplicate(context, account2);
        AddIFNotDuplicate(context, account3);
        AddIFNotDuplicate(context, account4);
        AddIFNotDuplicate(context, account5);
        AddIFNotDuplicate(context, account6);
        AddIFNotDuplicate(context, account7);
        AddIFNotDuplicate(context, account8);
        AddIFNotDuplicate(context, account9);

        AddIFNotDuplicate(context, appointment1);
        AddIFNotDuplicate(context, appointment2);
        AddIFNotDuplicate(context, appointment3);

        AddIFNotDuplicate(context, notInStock1);
        AddIFNotDuplicate(context, notInStock2);
        AddIFNotDuplicate(context, notInStock3);

        var workPlace1 = new DermatologistWorkPlace
        {
            WorkTime = new WorkTime
            {
                From = new DateTime(2020, 1, 1, 8, 0, 0),
                To = new DateTime(2020, 1, 1, 11, 0, 0),
            },
            DermatologistId = dermatologist1.Id,
            Pharmacy = pharmacy2
        };

        AddIFNotDuplicate(context, workPlace1);

        var supplierUser = new Supplier
        {
            Active = true,
            Address = new Address
            {
                Active = true,
                CreatedAt = DateTime.Now,
                City = "Zrenjanin",
                State = "Srbija",
                StreetName = "Prvomajska",
                StreetNumber = "19",
                Lat = 45.395820f,
                Lng = 20.397990f
            },
            CreatedAt = DateTime.Now,
            DateOfBirth = new DateTime(2000, 3, 6),
            FirstName = "The",
            LastName = "Ebay",
            PhoneNumber = "060123123123",
            PID = "1231231231231"
        };

        var supplierAccount = new Account
        {
            Active = true,
            CreatedAt = DateTime.Now,
            Email = "the.proebay@gmail.com",
            IsVerified = true,
            Password = "CD0C+ld3sdL7580LaAmI0Z2cpqebg6qsABQU9Rp2ZoM=",
            Salt = "Xc8pdY61u9IdCe8Ec9Pb2L+6J++zQNxMKbQ6SsgFqg8=",
            User = supplierUser,
            Role = Role.Supplier,
            Username = "theebay",
            ShouldChangePassword = false
        };

        AddIFNotDuplicate(context, supplierAccount);

        var supplierMedicine1 = new SupplierMedicine
        {
            Active = true,
            CreatedAt = DateTime.Now,
            SupplierId = supplierUser.Id,
            MedicineId = medicine1.Id,
            Quantity = 4
        };

        var supplierMedicine2 = new SupplierMedicine
        {
            Active = true,
            CreatedAt = DateTime.Now,
            SupplierId = supplierUser.Id,
            MedicineId = medicine2.Id,
            Quantity = 2
        };

        AddIFNotDuplicate(context, supplierMedicine1);
        AddIFNotDuplicate(context, supplierMedicine2);

        var pharmacyOrder1 = new PharmacyOrder
        {
            Active = true,
            CreatedAt = DateTime.Now,
            IsProcessed = true,
            PharmacyAdmin = pharmacyAdmin1,
            OffersDeadline = DateTime.Now.AddYears(10),
            Pharmacy = pharmacy1,
            OrderedMedicines = new List<OrderedMedicine>
            {
                new OrderedMedicine
                {
                    Medicine = medicine1,
                    Quantity = 2
                }
            }
        };

        var supplierOffer1 = new SupplierOffer
        {
            Id = new Guid("a0913d33-0b11-42a5-ad2f-4b7b4ef16ce8"),
            Active = true,
            CreatedAt = DateTime.Now,
            PharmacyOrder = pharmacyOrder1,
            DeliveryDeadline = 2,
            SupplierId = supplierUser.Id,
            Status = OfferStatus.WaitingForAnswer,
            TotalPrice = 1000
        };

        AddIFNotDuplicate(context, pharmacyOrder1);
        AddIFNotDuplicate(context, supplierOffer1);

        var pharmacyOrder2 = new PharmacyOrder
        {
            Active = true,
            CreatedAt = DateTime.Now,
            IsProcessed = false,
            PharmacyAdmin = pharmacyAdmin1,
            OffersDeadline = DateTime.Now.AddDays(-1),
            Pharmacy = pharmacy1,
            OrderedMedicines = new List<OrderedMedicine>
            {
                new OrderedMedicine
                {
                    Medicine = medicine1,
                    Quantity = 2
                }
            }
        };

        var supplierOffer2 = new SupplierOffer
        {
            Id = new Guid("72978073-01eb-4e6e-a623-1f5d41fda64d"),
            Active = true,
            CreatedAt = DateTime.Now,
            PharmacyOrder = pharmacyOrder2,
            DeliveryDeadline = 1,
            SupplierId = supplierUser.Id,
            Status = OfferStatus.WaitingForAnswer,
            TotalPrice = 1000
        };

        AddIFNotDuplicate(context, pharmacyOrder2);
        AddIFNotDuplicate(context, supplierOffer2);

        var pharmacyOrder3 = new PharmacyOrder
        {
            Active = true,
            CreatedAt = DateTime.Now,
            IsProcessed = false,
            PharmacyAdmin = pharmacyAdmin1,
            OffersDeadline = DateTime.Now.AddYears(10),
            Pharmacy = pharmacy1,
            OrderedMedicines = new List<OrderedMedicine>
            {
                new OrderedMedicine
                {
                    Medicine = medicine1,
                    Quantity = 2
                },
                new OrderedMedicine
                {
                    Medicine = medicine2,
                    Quantity = 3
                }
            }
        };

        var supplierOffer3 = new SupplierOffer
        {
            Id = new Guid("771ae913-8707-45e6-a4e6-3e71c21067f6"),
            Active = true,
            CreatedAt = DateTime.Now,
            PharmacyOrder = pharmacyOrder3,
            DeliveryDeadline = 3,
            SupplierId = supplierUser.Id,
            Status = OfferStatus.WaitingForAnswer,
            TotalPrice = 200
        };

        AddIFNotDuplicate(context, pharmacyOrder3);
        AddIFNotDuplicate(context, supplierOffer3);

        context.SaveChanges();
    }
}