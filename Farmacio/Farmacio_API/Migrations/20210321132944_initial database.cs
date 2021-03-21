using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class initialdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    StreetName = table.Column<string>(nullable: true),
                    StreetNumber = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Lat = table.Column<float>(nullable: false),
                    Lng = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ConsultationPoints = table.Column<int>(nullable: false),
                    ExaminationPoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyProgram",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    MinPoints = table.Column<int>(nullable: false),
                    Discount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyProgram", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicineType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyPriceList",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ExaminationPrice = table.Column<float>(nullable: false),
                    ConsultationPrice = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyPriceList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkTime",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    UniqueId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Form = table.Column<int>(nullable: false),
                    TypeId = table.Column<Guid>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    IsRecipeOnly = table.Column<bool>(nullable: false),
                    Contraindications = table.Column<string>(nullable: true),
                    AdditionalInfo = table.Column<string>(nullable: true),
                    RecommendedDose = table.Column<string>(nullable: true),
                    AverageGrade = table.Column<int>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Medicines_MedicineType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MedicineType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicinePoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    LoyaltyPointsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinePoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinePoints_LoyaltyPoints_LoyaltyPointsId",
                        column: x => x.LoyaltyPointsId,
                        principalTable: "LoyaltyPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicinePoints_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicinePrice",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    ActiveFrom = table.Column<DateTime>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: true),
                    PharmacyPriceListId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinePrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinePrice_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicinePrice_PharmacyPriceList_PharmacyPriceListId",
                        column: x => x.PharmacyPriceListId,
                        principalTable: "PharmacyPriceList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AddressId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PriceListId = table.Column<Guid>(nullable: true),
                    AverageGrade = table.Column<int>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pharmacies_PharmacyPriceList_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PharmacyPriceList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Discount = table.Column<int>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotion_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    PID = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    AddressId = table.Column<Guid>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    AverageGrade = table.Column<int>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: true),
                    Pharmacist_PharmacyId = table.Column<Guid>(nullable: true),
                    WorkTimeId = table.Column<Guid>(nullable: true),
                    Points = table.Column<int>(nullable: true),
                    NegativePoints = table.Column<int>(nullable: true),
                    LoyaltyProgramId = table.Column<Guid>(nullable: true),
                    PharmacyAdmin_PharmacyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_LoyaltyProgram_LoyaltyProgramId",
                        column: x => x.LoyaltyProgramId,
                        principalTable: "LoyaltyProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Pharmacies_Pharmacist_PharmacyId",
                        column: x => x.Pharmacist_PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_WorkTime_WorkTimeId",
                        column: x => x.WorkTimeId,
                        principalTable: "WorkTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Pharmacies_PharmacyAdmin_PharmacyId",
                        column: x => x.PharmacyAdmin_PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbsenceRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    RequesterId = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbsenceRequest_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbsenceRequest_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    ShouldChangePassword = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    WriterId = table.Column<Guid>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    IsAboutPharmacy = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complaints_Users_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DermatologistWorkPlace",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    WorkTimeId = table.Column<Guid>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: true),
                    DermatologistId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DermatologistWorkPlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DermatologistWorkPlace_Users_DermatologistId",
                        column: x => x.DermatologistId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DermatologistWorkPlace_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DermatologistWorkPlace_WorkTime_WorkTimeId",
                        column: x => x.WorkTimeId,
                        principalTable: "WorkTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERecipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    UniqueId = table.Column<string>(nullable: true),
                    IssuingDate = table.Column<DateTime>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ERecipes_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    PatientId = table.Column<string>(nullable: true),
                    MedicalStaffId = table.Column<Guid>(nullable: true),
                    MedicineId = table.Column<Guid>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grade_Users_MedicalStaffId",
                        column: x => x.MedicalStaffId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grade_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grade_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PatientId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredient_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: true),
                    PharmacyAdminId = table.Column<Guid>(nullable: true),
                    OffersDeadline = table.Column<DateTime>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyOrder_Users_PharmacyAdminId",
                        column: x => x.PharmacyAdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PharmacyOrder_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    UniqueId = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    PickupDeadline = table.Column<DateTime>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: true),
                    PatientId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    WriterId = table.Column<Guid>(nullable: true),
                    ComplaintId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplaintAnswer_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComplaintAnswer_Users_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERecipeMedicine",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    ERecipeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERecipeMedicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ERecipeMedicine_ERecipes_ERecipeId",
                        column: x => x.ERecipeId,
                        principalTable: "ERecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ERecipeMedicine_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    TherapyDurationInDays = table.Column<int>(nullable: false),
                    RecipeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Report_ERecipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "ERecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicineIngredient",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    IngredientId = table.Column<Guid>(nullable: true),
                    MassInMilligramms = table.Column<float>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineIngredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineIngredient_Ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicineIngredient_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderedMedicine",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: true),
                    PharmacyOrderId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedMedicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedMedicine_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderedMedicine_PharmacyOrder_PharmacyOrderId",
                        column: x => x.PharmacyOrderId,
                        principalTable: "PharmacyOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierOffer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SupplierId = table.Column<Guid>(nullable: true),
                    DeliveryDeadline = table.Column<DateTime>(nullable: false),
                    TotalPrice = table.Column<float>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PharmacyOrderId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierOffer_PharmacyOrder_PharmacyOrderId",
                        column: x => x.PharmacyOrderId,
                        principalTable: "PharmacyOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierOffer_Users_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservedMedicine",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: true),
                    ReservationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedMedicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservedMedicine_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservedMedicine_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    IsReserved = table.Column<bool>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: true),
                    MedicalStaffId = table.Column<Guid>(nullable: true),
                    ReportId = table.Column<Guid>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_MedicalStaffId",
                        column: x => x.MedicalStaffId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceRequest_PharmacyId",
                table: "AbsenceRequest",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceRequest_RequesterId",
                table: "AbsenceRequest",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MedicalStaffId",
                table: "Appointments",
                column: "MedicalStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PharmacyId",
                table: "Appointments",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ReportId",
                table: "Appointments",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintAnswer_ComplaintId",
                table: "ComplaintAnswer",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintAnswer_WriterId",
                table: "ComplaintAnswer",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_WriterId",
                table: "Complaints",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_DermatologistWorkPlace_DermatologistId",
                table: "DermatologistWorkPlace",
                column: "DermatologistId");

            migrationBuilder.CreateIndex(
                name: "IX_DermatologistWorkPlace_PharmacyId",
                table: "DermatologistWorkPlace",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_DermatologistWorkPlace_WorkTimeId",
                table: "DermatologistWorkPlace",
                column: "WorkTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_ERecipeMedicine_ERecipeId",
                table: "ERecipeMedicine",
                column: "ERecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ERecipeMedicine_MedicineId",
                table: "ERecipeMedicine",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_ERecipes_PatientId",
                table: "ERecipes",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_MedicalStaffId",
                table: "Grade",
                column: "MedicalStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_MedicineId",
                table: "Grade",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_PharmacyId",
                table: "Grade",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_PatientId",
                table: "Ingredient",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineIngredient_IngredientId",
                table: "MedicineIngredient",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineIngredient_MedicineId",
                table: "MedicineIngredient",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePoints_LoyaltyPointsId",
                table: "MedicinePoints",
                column: "LoyaltyPointsId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePoints_MedicineId",
                table: "MedicinePoints",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePrice_MedicineId",
                table: "MedicinePrice",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePrice_PharmacyPriceListId",
                table: "MedicinePrice",
                column: "PharmacyPriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_MedicineId",
                table: "Medicines",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_TypeId",
                table: "Medicines",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMedicine_MedicineId",
                table: "OrderedMedicine",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMedicine_PharmacyOrderId",
                table: "OrderedMedicine",
                column: "PharmacyOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_AddressId",
                table: "Pharmacies",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_PatientId",
                table: "Pharmacies",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_PriceListId",
                table: "Pharmacies",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyOrder_PharmacyAdminId",
                table: "PharmacyOrder",
                column: "PharmacyAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyOrder_PharmacyId",
                table: "PharmacyOrder",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_PharmacyId",
                table: "Promotion",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_RecipeId",
                table: "Report",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PatientId",
                table: "Reservations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PharmacyId",
                table: "Reservations",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedMedicine_MedicineId",
                table: "ReservedMedicine",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedMedicine_ReservationId",
                table: "ReservedMedicine",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOffer_PharmacyOrderId",
                table: "SupplierOffer",
                column: "PharmacyOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOffer_SupplierId",
                table: "SupplierOffer",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PharmacyId",
                table: "Users",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LoyaltyProgramId",
                table: "Users",
                column: "LoyaltyProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Pharmacist_PharmacyId",
                table: "Users",
                column: "Pharmacist_PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WorkTimeId",
                table: "Users",
                column: "WorkTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PharmacyAdmin_PharmacyId",
                table: "Users",
                column: "PharmacyAdmin_PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacies_Users_PatientId",
                table: "Pharmacies",
                column: "PatientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Pharmacies_PharmacyId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Pharmacies_Pharmacist_PharmacyId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Pharmacies_PharmacyAdmin_PharmacyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AbsenceRequest");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "ComplaintAnswer");

            migrationBuilder.DropTable(
                name: "DermatologistWorkPlace");

            migrationBuilder.DropTable(
                name: "ERecipeMedicine");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "MedicineIngredient");

            migrationBuilder.DropTable(
                name: "MedicinePoints");

            migrationBuilder.DropTable(
                name: "MedicinePrice");

            migrationBuilder.DropTable(
                name: "OrderedMedicine");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "ReservedMedicine");

            migrationBuilder.DropTable(
                name: "SupplierOffer");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Complaints");

            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "LoyaltyPoints");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "PharmacyOrder");

            migrationBuilder.DropTable(
                name: "ERecipes");

            migrationBuilder.DropTable(
                name: "MedicineType");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PharmacyPriceList");

            migrationBuilder.DropTable(
                name: "LoyaltyProgram");

            migrationBuilder.DropTable(
                name: "WorkTime");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
