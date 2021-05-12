using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_Tests.Migrations
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
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    MedicalStaffId = table.Column<Guid>(nullable: true),
                    MedicineId = table.Column<Guid>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
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
                    Name = table.Column<string>(nullable: true),
                    MinPoints = table.Column<int>(nullable: false),
                    Discount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyProgram", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicineIngredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MassInMilligrams = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineIngredients", x => x.Id);
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
                name: "PharmacyPriceLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: false),
                    ExaminationPrice = table.Column<float>(nullable: false),
                    ConsultationPrice = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyPriceLists", x => x.Id);
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
                name: "Pharmacies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AddressId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AverageGrade = table.Column<float>(nullable: false),
                    NumberOfGrades = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicinePoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false),
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
                    TypeId = table.Column<Guid>(nullable: false),
                    Manufacturer = table.Column<string>(nullable: true),
                    IsRecipeOnly = table.Column<bool>(nullable: false),
                    Contraindications = table.Column<string>(nullable: true),
                    AdditionalInfo = table.Column<string>(nullable: true),
                    RecommendedDose = table.Column<string>(nullable: true),
                    AverageGrade = table.Column<float>(nullable: false),
                    NumberOfGrades = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicines_MedicineType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MedicineType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DermatologistWorkPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DermatologistId = table.Column<Guid>(nullable: false),
                    WorkTimeId = table.Column<Guid>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DermatologistWorkPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DermatologistWorkPlaces_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DermatologistWorkPlaces_WorkTime_WorkTimeId",
                        column: x => x.WorkTimeId,
                        principalTable: "WorkTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Discount = table.Column<int>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotions_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    AddressId = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    AverageGrade = table.Column<float>(nullable: true),
                    NumberOfGrades = table.Column<int>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: true),
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
                        name: "FK_Users_LoyaltyProgram_LoyaltyProgramId",
                        column: x => x.LoyaltyProgramId,
                        principalTable: "LoyaltyProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_WorkTime_WorkTimeId",
                        column: x => x.WorkTimeId,
                        principalTable: "WorkTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Pharmacies_PharmacyAdmin_PharmacyId",
                        column: x => x.PharmacyAdmin_PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicinePrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PharmacyPriceListId = table.Column<Guid>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    ActiveFrom = table.Column<DateTime>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinePrices_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicinePrices_PharmacyPriceLists_PharmacyPriceListId",
                        column: x => x.PharmacyPriceListId,
                        principalTable: "PharmacyPriceLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicineReplacements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false),
                    ReplacementMedicineId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineReplacements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineReplacements_Medicines_ReplacementMedicineId",
                        column: x => x.ReplacementMedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotInStocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: false),
                    IsSeen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotInStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotInStocks_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAllergies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAllergies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAllergies_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyMedicines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyMedicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyMedicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierMedicines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SupplierId = table.Column<Guid>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierMedicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierMedicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbsenceRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    RequesterId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbsenceRequests_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbsenceRequests_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    WriterId = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    DermatologistId = table.Column<Guid>(nullable: true),
                    PharmacistId = table.Column<Guid>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complaints_Users_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaints_Users_DermatologistId",
                        column: x => x.DermatologistId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaints_Users_PharmacistId",
                        column: x => x.PharmacistId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaints_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    PatientId = table.Column<Guid>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ERecipes_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientPharmacyFollows",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPharmacyFollows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientPharmacyFollows_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPharmacyFollows_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: false),
                    PharmacyAdminId = table.Column<Guid>(nullable: false),
                    OffersDeadline = table.Column<DateTime>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyOrders_Users_PharmacyAdminId",
                        column: x => x.PharmacyAdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PharmacyOrders_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    PharmacyId = table.Column<Guid>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    WriterId = table.Column<Guid>(nullable: false),
                    ComplaintId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplaintAnswers_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplaintAnswers_Users_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ERecipeMedicine",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
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
                    ERecipeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Report_ERecipes_ERecipeId",
                        column: x => x.ERecipeId,
                        principalTable: "ERecipes",
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
                    PharmacyOrderId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedMedicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedMedicine_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderedMedicine_PharmacyOrders_PharmacyOrderId",
                        column: x => x.PharmacyOrderId,
                        principalTable: "PharmacyOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SupplierId = table.Column<Guid>(nullable: false),
                    DeliveryDeadline = table.Column<DateTime>(nullable: false),
                    TotalPrice = table.Column<float>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PharmacyOrderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierOffers_PharmacyOrders_PharmacyOrderId",
                        column: x => x.PharmacyOrderId,
                        principalTable: "PharmacyOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    MedicineId = table.Column<Guid>(nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
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
                    OriginalPrice = table.Column<float>(nullable: false),
                    IsReserved = table.Column<bool>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: true),
                    MedicalStaffId = table.Column<Guid>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_MedicalStaffId",
                        column: x => x.MedicalStaffId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceRequests_PharmacyId",
                table: "AbsenceRequests",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbsenceRequests_RequesterId",
                table: "AbsenceRequests",
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
                name: "IX_ComplaintAnswers_ComplaintId",
                table: "ComplaintAnswers",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintAnswers_WriterId",
                table: "ComplaintAnswers",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_WriterId",
                table: "Complaints",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_DermatologistId",
                table: "Complaints",
                column: "DermatologistId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_PharmacistId",
                table: "Complaints",
                column: "PharmacistId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_PharmacyId",
                table: "Complaints",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_DermatologistWorkPlaces_PharmacyId",
                table: "DermatologistWorkPlaces",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_DermatologistWorkPlaces_WorkTimeId",
                table: "DermatologistWorkPlaces",
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
                name: "IX_MedicinePoints_LoyaltyPointsId",
                table: "MedicinePoints",
                column: "LoyaltyPointsId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePrices_MedicineId",
                table: "MedicinePrices",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePrices_PharmacyPriceListId",
                table: "MedicinePrices",
                column: "PharmacyPriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineReplacements_ReplacementMedicineId",
                table: "MedicineReplacements",
                column: "ReplacementMedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_TypeId",
                table: "Medicines",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotInStocks_MedicineId",
                table: "NotInStocks",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMedicine_MedicineId",
                table: "OrderedMedicine",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMedicine_PharmacyOrderId",
                table: "OrderedMedicine",
                column: "PharmacyOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergies_MedicineId",
                table: "PatientAllergies",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPharmacyFollows_PatientId",
                table: "PatientPharmacyFollows",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPharmacyFollows_PharmacyId",
                table: "PatientPharmacyFollows",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_AddressId",
                table: "Pharmacies",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMedicines_MedicineId",
                table: "PharmacyMedicines",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyOrders_PharmacyAdminId",
                table: "PharmacyOrders",
                column: "PharmacyAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyOrders_PharmacyId",
                table: "PharmacyOrders",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_PharmacyId",
                table: "Promotions",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_ERecipeId",
                table: "Report",
                column: "ERecipeId");

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
                name: "IX_SupplierMedicines_MedicineId",
                table: "SupplierMedicines",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOffers_PharmacyOrderId",
                table: "SupplierOffers",
                column: "PharmacyOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LoyaltyProgramId",
                table: "Users",
                column: "LoyaltyProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PharmacyId",
                table: "Users",
                column: "PharmacyId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbsenceRequests");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "ComplaintAnswers");

            migrationBuilder.DropTable(
                name: "DermatologistWorkPlaces");

            migrationBuilder.DropTable(
                name: "ERecipeMedicine");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "MedicineIngredients");

            migrationBuilder.DropTable(
                name: "MedicinePoints");

            migrationBuilder.DropTable(
                name: "MedicinePrices");

            migrationBuilder.DropTable(
                name: "MedicineReplacements");

            migrationBuilder.DropTable(
                name: "NotInStocks");

            migrationBuilder.DropTable(
                name: "OrderedMedicine");

            migrationBuilder.DropTable(
                name: "PatientAllergies");

            migrationBuilder.DropTable(
                name: "PatientPharmacyFollows");

            migrationBuilder.DropTable(
                name: "PharmacyMedicines");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "ReservedMedicine");

            migrationBuilder.DropTable(
                name: "SupplierMedicines");

            migrationBuilder.DropTable(
                name: "SupplierOffers");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Complaints");

            migrationBuilder.DropTable(
                name: "LoyaltyPoints");

            migrationBuilder.DropTable(
                name: "PharmacyPriceLists");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "PharmacyOrders");

            migrationBuilder.DropTable(
                name: "ERecipes");

            migrationBuilder.DropTable(
                name: "MedicineType");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LoyaltyProgram");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "WorkTime");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
