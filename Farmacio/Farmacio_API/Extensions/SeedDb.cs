using Farmacio_Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

public static class SeedDb
{
    public static IHost SeedDbContext<TContext>(this IHost host) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetService<TContext>();

            try
            {
                Seed(context);
            }
            catch (Exception)
            {
            }
        }

        return host;
    }

    private static void Seed<TContext>(TContext context) where TContext : DbContext
    {
        Address address1 = new Address
        {
            Id = new Guid("9aa8c06c-792f-4eeb-8d0f-c6b534d8aab8"),
            StreetName = "Jevrejska",
            StreetNumber = "14a",
            City = "Novi Sad",
            State = "Srbija",
            Lat = 45.254410f,
            Lng = 19.842550f
        };

        Address address2 = new Address
        {
            Id = new Guid("3e8fb8d6-c5be-4c39-acc5-11734d5ac15c"),
            StreetName = "Zmaj Jovina",
            StreetNumber = "27a",
            City = "Novi Sad",
            State = "Srbija",
            Lat = 45.254410f,
            Lng = 19.842550f
        };

        Address address3 = new Address
        {
            Id = new Guid("ea68b7cd-271e-40e6-a4c9-b2382411d421"),
            CreatedAt = DateTime.Now,
            Active = true,
            StreetName = "Bulevar Oslobodjenja",
            StreetNumber = "71a",
            City = "Novi Sad",
            State = "Srbija",
            Lat = 45.254410f,
            Lng = 19.842550f
        };

        PharmacyPriceList priceList1 = new PharmacyPriceList
        {
            Id = new Guid("4f4b8d6b-999f-4342-8054-00c382bb2155"),
            ExaminationPrice = 1500f,
            ConsultationPrice = 800f,
            MedicinePriceList = new List<MedicinePrice>()
        };

        PharmacyPriceList priceList2 = new PharmacyPriceList
        {
            Id = new Guid("04ebe74c-92aa-4130-8569-92a5cbcc8eb9"),
            ExaminationPrice = 1200f,
            ConsultationPrice = 600f,
            MedicinePriceList = new List<MedicinePrice>()
        };

        PharmacyPriceList priceList3 = new PharmacyPriceList
        {
            Id = new Guid("d74fecbd-f907-4827-99fa-73fe63d49e3e"),
            ExaminationPrice = 1300f,
            ConsultationPrice = 750f,
            MedicinePriceList = new List<MedicinePrice>()
        };

        Pharmacy pharmacy1 = new Pharmacy
        {
            Id = new Guid("874282f3-3df9-4ee0-bf7e-33d9ba3ec456"),
            Name = "BENU Apoteka",
            Address = address1,
            Description = "Apotekarska ustanova BENU je najveci lanac apoteka u Srbiji i deo je velike medjunarodne kompanije PHOENIX iz Nemacke.",
            Pharmacists = new List<Pharmacist>(),
            Dermatologists = new List<Dermatologist>(),
            PriceList = priceList1,
            Orders = new List<PharmacyOrder>(),
            Promotions = new List<Promotion>(),
            AverageGrade = 0,
            Grades = new List<Grade>()
        };

        Pharmacy pharmacy2 = new Pharmacy
        {
            Id = new Guid("1f833fb4-445b-4a87-bfc7-9b83bb788259"),
            Name = "Viva Farm",
            Address = address2,
            Description = "Tu smo da zajedno sa vama utièemo na oèuvanje dobog zdravlja i spreèavanje razvoja bolesti za koje postoji rizik ili predispozicija.",
            Pharmacists = new List<Pharmacist>(),
            Dermatologists = new List<Dermatologist>(),
            PriceList = priceList2,
            Orders = new List<PharmacyOrder>(),
            Promotions = new List<Promotion>(),
            AverageGrade = 0,
            Grades = new List<Grade>()
        };

        Pharmacy pharmacy3 = new Pharmacy
        {
            Id = new Guid("3eb2dfab-1b1c-4ff4-98cf-8fd2fb3201d7"),
            Name = "Dr.Max",
            Address = address3,
            Description = "Dr.Max je meðunarodni lanac apoteka, koji je prisutan u 6 " +
            "zemalja Centralno Istoène Evrope sa preko 2000 apoteka i 12000 zaposlenih. Od 2017. godine, Dr.Max je prisutan i na tržištu Srbije.",
            Pharmacists = new List<Pharmacist>(),
            Dermatologists = new List<Dermatologist>(),
            PriceList = priceList3,
            Orders = new List<PharmacyOrder>(),
            Promotions = new List<Promotion>(),
            AverageGrade = 0,
            Grades = new List<Grade>()
        };

        MedicineType medicineType1 = new MedicineType
        {
            Id = new Guid("699fec99-9fd3-4354-ad27-3e75a0816890"),
            TypeName = "Analgetik"
        };

        MedicineIngredient medicineIngridient1 = new MedicineIngredient
        {
            Id = new Guid("2d23c231-d94e-462a-87dc-e5872b360bcd"),
            Ingredient = new Ingredient
            {
                Id = new Guid("d3442f20-dd12-4620-906d-7acb83ee5492"),
                Name = "kroskarmeloza-natrijum"
            },
            MassInMilligramms = 1
        };

        MedicineIngredient medicineIngridient2 = new MedicineIngredient
        {
            Id = new Guid("8a0ec799-e693-4d86-a133-363b3203b736"),
            Ingredient = new Ingredient
            {
                Id = new Guid("3fa1a6a0-7e99-4329-88c3-a8fdb6957821"),
                Name = "paracetamol"
            },
            MassInMilligramms = 500
        };

        List<MedicineIngredient> ingredients1 = new List<MedicineIngredient>();
        ingredients1.Add(medicineIngridient1);
        ingredients1.Add(medicineIngridient2);

        MedicineIngredient medicineIngridient3 = new MedicineIngredient
        {
            Id = new Guid("e3df4532-df69-4776-a22d-1e9d639baf3b"),
            Ingredient = new Ingredient
            {
                Id = new Guid("b09bb567-7d98-484a-9634-a0120cf0a952"),
                Name = "ibuprofena"
            },
            MassInMilligramms = 400
        };

        MedicineIngredient medicineIngridient4 = new MedicineIngredient
        {
            Id = new Guid("a3a4c1c1-8962-4df0-b271-898c7cdfb452"),
            Ingredient = new Ingredient
            {
                Id = new Guid("39b6c5ac-7df0-4c5b-89b6-a1b81b211b2a"),
                Name = "laktoza"
            },
            MassInMilligramms = 10
        };

        List<MedicineIngredient> ingredients2 = new List<MedicineIngredient>();
        ingredients1.Add(medicineIngridient1);
        ingredients1.Add(medicineIngridient3);
        ingredients1.Add(medicineIngridient4);

        List<MedicineIngredient> ingredients3 = new List<MedicineIngredient>();
        ingredients1.Add(medicineIngridient2);
        ingredients1.Add(medicineIngridient3);
        ingredients1.Add(medicineIngridient4);

        Medicine medicine1 = new Medicine
        {
            Id = new Guid("ce512ff8-3927-43cf-8ae9-33a441b98ea1"),
            UniqueId = "123abc",
            Name = "Paracetamol",
            Form = MedicineForm.Tablet,
            Type = medicineType1,
            Manufacturer = "Hemofarm",
            IsRecipeOnly = false,
            Contraindications = "Kožni osip, alergijse reakcije, povišen krvni pritisak, glavobolju, vrtoglavicu,stomaène tegobe(muènina ili proliv), " +
            "probleme sa spavanjem, lupanje srca i poremeæaje krvitrombocitopenija i agranulocitoza(smanjenje broja krvnih ploèica i belih krvnih " +
            "zrnaca koji se javljaj kod èeste i dugotrajne upotrebe)",
            AdditionalInfo = "Izgled tableta: okrugle tablete, bele boje, koje sa jedne strane imaju utisnutu podeonu crtu. ",
            RecommendedDose = "2 do 4 tablete dnevno",
            MedicineIngredients = ingredients1,
            Replacements = new List<Medicine>(),
            AverageGrade = 0,
            Grades = new List<Grade>()
        };

        Medicine medicine2 = new Medicine
        {
            Id = new Guid("fe12af31-de77-4f2b-b2a5-11c6d6a340f5"),
            UniqueId = "321cba",
            Name = "Brufen",
            Form = MedicineForm.Tablet,
            Type = medicineType1,
            Manufacturer = "Galenika",
            IsRecipeOnly = false,
            Contraindications = "Glavobolja, vrtoglavica, gastrointestinalna neželjena dejstva (loše varenje, dijareja, muènina, povraæanje, bol u stomaku, " +
            "nadimanje, konstipacija, crna stolica, krvarenje u stomaku i crevima, povraæanje krvi), osip, zamor",
            AdditionalInfo = "Izgled: bela, ovalna, bikonveksna film tableta",
            RecommendedDose = "Uobièajena doza je 600-1800 mg na dan podeljena u nekoliko doza",
            MedicineIngredients = ingredients2,
            Replacements = new List<Medicine>(),
            AverageGrade = 0,
            Grades = new List<Grade>()
        };

        Medicine medicine3 = new Medicine
        {
            Id = new Guid("7e706303-f265-4091-95e2-bbdd5fa8ca65"),
            UniqueId = "321cba",
            Name = "Caffetin",
            Form = MedicineForm.Tablet,
            Type = medicineType1,
            Manufacturer = "Hemofarm",
            IsRecipeOnly = false,
            Contraindications = "Gadjenje, povracanje, nesanica, lupanje srca ili ubrzan rad srca, porecemaji funkcije jetre i bubrega, zavisnost.",
            AdditionalInfo = "Izgled: bela, ovalna, ravna.",
            RecommendedDose = "3-4 tablete dnevno",
            MedicineIngredients = ingredients2,
            Replacements = new List<Medicine>(),
            AverageGrade = 0,
            Grades = new List<Grade>()
        };

        List<Ingredient> allergies1 = new List<Ingredient>();
        allergies1.Add(new Ingredient
        {
            Id = new Guid("00537083-519d-460e-9d4e-c37ed100a3b4"),
            CreatedAt = DateTime.Now,
            Active = true,
            Name = "ibuprofena"
        });

        Patient patient1 = new Patient
        {
            Id = new Guid("2133bc63-1505-4835-9a40-124993d53be2"),
            FirstName = "Pera",
            LastName = "Peric",
            DateOfBirth = DateTime.Now,
            PID = "1234567891234",
            PhoneNumber = "06456987412",
            Address = address1,
            Points = 0,
            NegativePoints = 0,
            LoyaltyProgram = null,
            FollowedPharmacies = new List<Pharmacy>(),
            Appointments = new List<Appointment>(),
            Allergies = allergies1
        };

        Account account1 = new Account
        {
            Id = new Guid("d1cb8425-c01f-4552-8660-75910e0def59"),
            Username = "pera",
            Password = "pera123",
            Salt = "",
            Email = "pera.peric@gmail.com",
            Role = Role.Patient,
            IsVerified = true,
            ShouldChangePassword = false,
            User = patient1
        };

        Patient patient2 = new Patient
        {
            Id = new Guid("25bcb39f-8059-4200-b1c2-a09410d42fa3"),
            FirstName = "Mikic",
            LastName = "Mikic",
            DateOfBirth = DateTime.Now,
            PID = "7894561233216",
            PhoneNumber = "06145789631",
            Address = address2,
            Points = 0,
            NegativePoints = 1,
            LoyaltyProgram = null,
            FollowedPharmacies = new List<Pharmacy>(),
            Appointments = new List<Appointment>(),
            Allergies = new List<Ingredient>()
        };

        Account account2 = new Account
        {
            Id = new Guid("4056ba01-8b87-4319-8e32-ebc1471e3810"),
            Username = "mika",
            Password = "mika123",
            Salt = "",
            Email = "mika.mika@gmail.com",
            Role = Role.Patient,
            IsVerified = true,
            ShouldChangePassword = false,
            User = patient2
        };

        Patient patient3 = new Patient
        {
            Id = new Guid("1311e2b4-536f-4f95-aa6c-1f7547e00f28"),
            FirstName = "Janko",
            LastName = "Jankovic",
            DateOfBirth = DateTime.Now,
            PID = "4561239874561",
            PhoneNumber = "0654789123",
            Address = address3,
            Points = 0,
            NegativePoints = 0,
            LoyaltyProgram = null,
            FollowedPharmacies = new List<Pharmacy>(),
            Appointments = new List<Appointment>(),
            Allergies = new List<Ingredient>()
        };

        Account account3 = new Account
        {
            Id = new Guid("38bf9e22-6604-4063-a354-d8e0d7d4c06c"),
            Username = "janko",
            Password = "janko123",
            Salt = "",
            Email = "janko.jankovic@gmail.com",
            Role = Role.Patient,
            IsVerified = true,
            ShouldChangePassword = false,
            User = patient3
        };

        SystemAdmin systemAdmin1 = new SystemAdmin
        {
            Id = new Guid("38f7bc38-f900-4bbf-91ea-f65272498bca"),
            FirstName = "Sistemko",
            LastName = "Adminic",
            DateOfBirth = DateTime.Now.AddYears(-35),
            PID = "9852356231569",
            PhoneNumber = "0641236547",
            Address = address2,
        };

        Account account4 = new Account
        {
            Id = new Guid("e1444625-7f0f-45d5-afbe-0738d8a5cf49"),
            Username = "sysadmin",
            Password = "admin123",
            Salt = "",
            Email = "sistem.admin@gmail.com",
            Role = Role.SystemAdmin,
            IsVerified = true,
            ShouldChangePassword = false,
            User = systemAdmin1
        };

        PharmacyAdmin pharmacyAdmin1 = new PharmacyAdmin
        {
            Id = new Guid("10ef9b01-abfe-427d-8b0b-ef8f5ea90450"),
            FirstName = "Farmaci",
            LastName = "Adminic",
            DateOfBirth = DateTime.Now.AddYears(-36),
            PID = "9842358961589",
            PhoneNumber = "0647984523",
            Address = address1,
            Pharmacy = pharmacy1
        };

        Account account5 = new Account
        {
            Id = new Guid("5328353c-532a-4eeb-ac66-291267e8813c"),
            Username = "pharmadmin",
            Password = "phadmin123",
            Salt = "",
            Email = "pharmacy1admin@gmail.com",
            Role = Role.PharmacyAdmin,
            IsVerified = true,
            ShouldChangePassword = false,
            User = pharmacyAdmin1
        };

        Pharmacist pharmacist1 = new Pharmacist
        {
            Id = new Guid("40f82145-8297-43f9-bcb8-f47685afcc6a"),
            FirstName = "Petar",
            LastName = "Petrovic",
            DateOfBirth = DateTime.Now.AddYears(-39),
            PID = "9822359563218",
            PhoneNumber = "0648521694",
            Address = address2,
            Pharmacy = pharmacy1,
            WorkTime = new WorkTime
            {
                Id = new Guid("12f70fd4-1899-45e7-8687-85e740f588c8"),
                From = new DateTime(2020, 1, 1, 7, 0, 0),
                To = new DateTime(2020, 1, 1, 13, 0, 0),
            },
            AbsenceRequests = new List<AbsenceRequest>(),
            Appointments = new List<Appointment>(),
            AverageGrade = 0,
            Grades = new List<Grade>()
        };

        Account account6 = new Account
        {
            Id = new Guid("b471c7a4-cad5-40ba-8929-d3c8bc850378"),
            Username = "ppetar",
            Password = "petrovic123",
            Salt = "",
            Email = "petar82petrovic@gmail.com",
            Role = Role.Pharmacist,
            IsVerified = true,
            ShouldChangePassword = false,
            User = pharmacist1
        };

        Pharmacist pharmacist2 = new Pharmacist
        {
            Id = new Guid("ed7dd486-9976-4de3-923a-e9d4c2ac84a8"),
            FirstName = "Jelena",
            LastName = "Petrovic",
            DateOfBirth = DateTime.Now.AddYears(-27),
            PID = "9912125626598",
            PhoneNumber = "0647512493",
            Address = address3,
            Pharmacy = null,
            WorkTime = null,
            AbsenceRequests = new List<AbsenceRequest>(),
            Appointments = new List<Appointment>(),
            AverageGrade = 0,
            Grades = new List<Grade>()
        };

        Account account7 = new Account
        {
            Id = new Guid("2290c110-a476-4b51-9978-741dd200f65f"),
            Username = "pjelena",
            Password = "jelena123",
            Salt = "",
            Email = "jelpetrovic@gmail.com",
            Role = Role.Pharmacist,
            IsVerified = true,
            ShouldChangePassword = false,
            User = pharmacist2
        };

        Dermatologist dermatologist1 = new Dermatologist
        {
            Id = new Guid("105ac38f-988b-4508-b4fa-9e49720c9f15"),
            FirstName = "Milica",
            LastName = "Mikic",
            DateOfBirth = DateTime.Now.AddYears(-35),
            PID = "9872125346751",
            PhoneNumber = "0649467315",
            Address = address2,
            AbsenceRequests = new List<AbsenceRequest>(),
            Appointments = new List<Appointment>(),
            AverageGrade = 0,
            Grades = new List<Grade>(),
            WorkPlaces = new List<DermatologistWorkPlace>
            {
                new DermatologistWorkPlace
                {
                    WorkTime = new WorkTime
                    {
                        Id = new Guid("b2d27c26-0ed4-4f7e-8c21-88df75766bed"),
                        From = new DateTime(2020, 1, 1, 8, 0, 0),
                        To = new DateTime(2020, 1, 1, 11, 0, 0),
                    },
                    Id = new Guid("23a9f826-b8f6-4146-895a-0197ae1c681d"),
                    Pharmacy = pharmacy2
                },
            }
        };

        Account account8 = new Account
        {
            Id = new Guid("47e70b60-8d7f-4931-a120-07da93d62085"),
            Username = "mmilica",
            Password = "milica123",
            Salt = "",
            Email = "milicamikic@gmail.com",
            Role = Role.Dermatologist,
            IsVerified = true,
            ShouldChangePassword = false,
            User = dermatologist1
        };

        Dermatologist dermatologist2 = new Dermatologist
        {
            Id = new Guid("58714578-0ced-44ee-a1b2-2b924964fbb1"),
            FirstName = "Milos",
            LastName = "Mikic",
            DateOfBirth = DateTime.Now.AddYears(-37),
            PID = "9852346197521",
            PhoneNumber = "0649467312",
            Address = address1,
            AbsenceRequests = new List<AbsenceRequest>(),
            Appointments = new List<Appointment>(),
            AverageGrade = 0,
            Grades = new List<Grade>(),
            WorkPlaces = new List<DermatologistWorkPlace>()
        };

        Account account9 = new Account
        {
            Id = new Guid("1284ac82-a194-448f-86ea-113a9f81e324"),
            Username = "mmilos",
            Password = "milos123",
            Salt = "",
            Email = "milosmikic85@gmail.com",
            Role = Role.Dermatologist,
            IsVerified = true,
            ShouldChangePassword = false,
            User = dermatologist2
        };
    }
}