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
                CreatedAt = DateTime.Now,
                Active = true,
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
                CreatedAt = DateTime.Now,
                Active = true,
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
                CreatedAt = DateTime.Now,
                Active = true,
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
                CreatedAt = DateTime.Now,
                Active = true,
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
            Id = new Guid("be7abcbc-4eaa-4596-965d-7c97c2fcbaf3"),
            Username = "janko",
            Password = "janko123",
            Salt = "",
            Email = "janko.jankovic@gmail.com",
            Role = Role.Patient,
            IsVerified = true,
            ShouldChangePassword = false,
            User = patient3
        };
    }
}