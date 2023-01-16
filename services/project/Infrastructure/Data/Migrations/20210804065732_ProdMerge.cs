using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Data.Migrations
{
    public partial class ProdMerge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Content = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationStartupImageDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BackgroundImage = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    ForegroundImage = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStartupImageDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationTerminationDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Terminated = table.Column<bool>(type: "boolean", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTerminationDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Image = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    ExtUrl = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    IsDeletable = table.Column<bool>(type: "boolean", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    Username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryTermsDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTermsDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstagramUrlDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramUrlDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileNotificationsByCity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Content = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Image = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileNotificationsByCity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileNotificationsByPriceGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Content = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Image = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileNotificationsByPriceGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VacanciesDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacanciesDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VkUrlDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VkUrlDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Surname = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    Email = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Password = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleEn = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    TitleRu = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: true),
                    Content = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuProducts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientLoginRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<long>(type: "bigint", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InvalidAttempts = table.Column<long>(type: "bigint", nullable: false),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLoginRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLoginRequests_ClientAccounts_ClientAccountId",
                        column: x => x.ClientAccountId,
                        principalTable: "ClientAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAddresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Floor = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Street = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Home = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Flat = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Entrance = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Comment = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryAddresses_ClientAccounts_ClientAccountId",
                        column: x => x.ClientAccountId,
                        principalTable: "ClientAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    PriceGroupId = table.Column<long>(type: "bigint", nullable: false),
                    GmtOffsetFromMoscow = table.Column<int>(type: "integer", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_PriceGroups_PriceGroupId",
                        column: x => x.PriceGroupId,
                        principalTable: "PriceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MobilePushToPriceGroup",
                columns: table => new
                {
                    MobilePushByPriceGroupId = table.Column<long>(type: "bigint", nullable: false),
                    PriceGroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobilePushToPriceGroup", x => new { x.PriceGroupId, x.MobilePushByPriceGroupId });
                    table.ForeignKey(
                        name: "FK_MobilePushToPriceGroup_MobileNotificationsByPriceGroup_Mobi~",
                        column: x => x.MobilePushByPriceGroupId,
                        principalTable: "MobileNotificationsByPriceGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MobilePushToPriceGroup_PriceGroups_PriceGroupId",
                        column: x => x.PriceGroupId,
                        principalTable: "PriceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TokenSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    WorkerAccountId = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenSessions_WorkerAccounts_WorkerAccountId",
                        column: x => x.WorkerAccountId,
                        principalTable: "WorkerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerAccountToRole",
                columns: table => new
                {
                    WorkerAccountId = table.Column<long>(type: "bigint", nullable: false),
                    WorkerRoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerAccountToRole", x => new { x.WorkerAccountId, x.WorkerRoleId });
                    table.ForeignKey(
                        name: "FK_WorkerAccountToRole_WorkerAccounts_WorkerAccountId",
                        column: x => x.WorkerAccountId,
                        principalTable: "WorkerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerAccountToRole_WorkerRoles_WorkerRoleId",
                        column: x => x.WorkerRoleId,
                        principalTable: "WorkerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientAccountId = table.Column<long>(type: "bigint", nullable: false),
                    MenuItemId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_ClientAccounts_ClientAccountId",
                        column: x => x.ClientAccountId,
                        principalTable: "ClientAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientAccountId = table.Column<long>(type: "bigint", nullable: false),
                    MenuItemId = table.Column<long>(type: "bigint", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteItems_ClientAccounts_ClientAccountId",
                        column: x => x.ClientAccountId,
                        principalTable: "ClientAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuCPFCs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Calories = table.Column<float>(type: "real", nullable: false),
                    Proteins = table.Column<float>(type: "real", nullable: false),
                    Fats = table.Column<float>(type: "real", nullable: false),
                    Carbohydrates = table.Column<float>(type: "real", nullable: false),
                    MenuItemId = table.Column<long>(type: "bigint", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCPFCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuCPFCs_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemMeasure",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenuItemId = table.Column<long>(type: "bigint", nullable: false),
                    MeasureType = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemMeasure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItemMeasure_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemToPriceGroup",
                columns: table => new
                {
                    PriceGroupId = table.Column<long>(type: "bigint", nullable: false),
                    MenuItemId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemToPriceGroup", x => new { x.PriceGroupId, x.MenuItemId });
                    table.ForeignKey(
                        name: "FK_MenuItemToPriceGroup_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemToPriceGroup_PriceGroups_PriceGroupId",
                        column: x => x.PriceGroupId,
                        principalTable: "PriceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemToMenuProduct",
                columns: table => new
                {
                    MenuItemId = table.Column<long>(type: "bigint", nullable: false),
                    MenuProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemToMenuProduct", x => new { x.MenuProductId, x.MenuItemId });
                    table.ForeignKey(
                        name: "FK_MenuItemToMenuProduct_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemToMenuProduct_MenuProducts_MenuProductId",
                        column: x => x.MenuProductId,
                        principalTable: "MenuProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAddressLatLngs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeliveryAddressId = table.Column<long>(type: "bigint", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Lat = table.Column<float>(type: "real", nullable: false),
                    Lng = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddressLatLngs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryAddressLatLngs_DeliveryAddresses_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "DeliveryAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BannerToCity",
                columns: table => new
                {
                    BannerId = table.Column<long>(type: "bigint", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerToCity", x => new { x.BannerId, x.CityId });
                    table.ForeignKey(
                        name: "FK_BannerToCity_Banners_BannerId",
                        column: x => x.BannerId,
                        principalTable: "Banners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannerToCity_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityLatLngs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Lat = table.Column<float>(type: "real", nullable: false),
                    Lng = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityLatLngs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityLatLngs_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MobilePushToCity",
                columns: table => new
                {
                    MobilePushByCityId = table.Column<long>(type: "bigint", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobilePushToCity", x => new { x.CityId, x.MobilePushByCityId });
                    table.ForeignKey(
                        name: "FK_MobilePushToCity_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MobilePushToCity_MobileNotificationsByCity_MobilePushByCity~",
                        column: x => x.MobilePushByCityId,
                        principalTable: "MobileNotificationsByCity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Address = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurants_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryStops",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RestaurantId = table.Column<long>(type: "bigint", nullable: false),
                    IssuerId = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryStops_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryStops_WorkerAccounts_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "WorkerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryTimeOpenCloses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DayOfWeek = table.Column<long>(type: "bigint", nullable: false),
                    Open = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Close = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IsWorking = table.Column<bool>(type: "boolean", nullable: false),
                    RestaurantId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTimeOpenCloses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryTimeOpenCloses_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    CreatorWorkerAccountId = table.Column<long>(type: "bigint", nullable: false),
                    ClientAccountId = table.Column<long>(type: "bigint", nullable: false),
                    RestaurantId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAtDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PickupType = table.Column<long>(type: "bigint", nullable: false),
                    DeliveryAddressId = table.Column<long>(type: "bigint", nullable: true),
                    DeliveredAtDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DelayType = table.Column<long>(type: "bigint", nullable: false),
                    AwaitedAtDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PaymentType = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Promocode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_ClientAccounts_ClientAccountId",
                        column: x => x.ClientAccountId,
                        principalTable: "ClientAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_DeliveryAddresses_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "DeliveryAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_WorkerAccounts_CreatorWorkerAccountId",
                        column: x => x.CreatorWorkerAccountId,
                        principalTable: "WorkerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickupStops",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RestaurantId = table.Column<long>(type: "bigint", nullable: false),
                    IssuerId = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupStops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickupStops_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickupStops_WorkerAccounts_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "WorkerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickupTimeOpenCloses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DayOfWeek = table.Column<long>(type: "bigint", nullable: false),
                    Open = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Close = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IsWorking = table.Column<bool>(type: "boolean", nullable: false),
                    RestaurantId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupTimeOpenCloses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickupTimeOpenCloses_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantLatLngs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RestaurantId = table.Column<long>(type: "bigint", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Lat = table.Column<float>(type: "real", nullable: false),
                    Lng = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantLatLngs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantLatLngs_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerAccountToRestaurant",
                columns: table => new
                {
                    WorkerAccountId = table.Column<long>(type: "bigint", nullable: false),
                    RestaurantId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerAccountToRestaurant", x => new { x.WorkerAccountId, x.RestaurantId });
                    table.ForeignKey(
                        name: "FK_WorkerAccountToRestaurant_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerAccountToRestaurant_WorkerAccounts_WorkerAccountId",
                        column: x => x.WorkerAccountId,
                        principalTable: "WorkerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZoneLatLngs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RestaurantId = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Lat = table.Column<float>(type: "real", nullable: false),
                    Lng = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneLatLngs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneLatLngs_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    MenuItemId = table.Column<long>(type: "bigint", nullable: false),
                    PurchasePrice = table.Column<float>(type: "real", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutDatas_IsSoftDeleted",
                table: "AboutDatas",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStartupImageDatas_IsSoftDeleted",
                table: "ApplicationStartupImageDatas",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTerminationDatas_IsSoftDeleted",
                table: "ApplicationTerminationDatas",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_IsSoftDeleted",
                table: "Banners",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_BannerToCity_CityId",
                table: "BannerToCity",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ClientAccountId",
                table: "CartItems",
                column: "ClientAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_IsSoftDeleted",
                table: "CartItems",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_MenuItemId",
                table: "CartItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsSoftDeleted",
                table: "Categories",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_IsSoftDeleted",
                table: "Cities",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_PriceGroupId",
                table: "Cities",
                column: "PriceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CityLatLngs_CityId",
                table: "CityLatLngs",
                column: "CityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CityLatLngs_IsSoftDeleted",
                table: "CityLatLngs",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAccounts_IsSoftDeleted",
                table: "ClientAccounts",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLoginRequests_ClientAccountId",
                table: "ClientLoginRequests",
                column: "ClientAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLoginRequests_IsSoftDeleted",
                table: "ClientLoginRequests",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddresses_ClientAccountId",
                table: "DeliveryAddresses",
                column: "ClientAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddresses_IsSoftDeleted",
                table: "DeliveryAddresses",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddressLatLngs_DeliveryAddressId",
                table: "DeliveryAddressLatLngs",
                column: "DeliveryAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddressLatLngs_IsSoftDeleted",
                table: "DeliveryAddressLatLngs",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStops_IsSoftDeleted",
                table: "DeliveryStops",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStops_IssuerId",
                table: "DeliveryStops",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStops_RestaurantId",
                table: "DeliveryStops",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTermsDatas_IsSoftDeleted",
                table: "DeliveryTermsDatas",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTimeOpenCloses_IsSoftDeleted",
                table: "DeliveryTimeOpenCloses",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTimeOpenCloses_RestaurantId",
                table: "DeliveryTimeOpenCloses",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteItems_ClientAccountId",
                table: "FavoriteItems",
                column: "ClientAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteItems_IsSoftDeleted",
                table: "FavoriteItems",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteItems_MenuItemId",
                table: "FavoriteItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramUrlDatas_IsSoftDeleted",
                table: "InstagramUrlDatas",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCPFCs_IsSoftDeleted",
                table: "MenuCPFCs",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCPFCs_MenuItemId",
                table: "MenuCPFCs",
                column: "MenuItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemMeasure_IsSoftDeleted",
                table: "MenuItemMeasure",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemMeasure_MenuItemId",
                table: "MenuItemMeasure",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryId",
                table: "MenuItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_IsSoftDeleted",
                table: "MenuItems",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemToMenuProduct_MenuItemId",
                table: "MenuItemToMenuProduct",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemToPriceGroup_MenuItemId",
                table: "MenuItemToPriceGroup",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuProducts_CategoryId",
                table: "MenuProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuProducts_IsSoftDeleted",
                table: "MenuProducts",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MobileNotificationsByCity_IsSoftDeleted",
                table: "MobileNotificationsByCity",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MobileNotificationsByPriceGroup_IsSoftDeleted",
                table: "MobileNotificationsByPriceGroup",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MobilePushToCity_MobilePushByCityId",
                table: "MobilePushToCity",
                column: "MobilePushByCityId");

            migrationBuilder.CreateIndex(
                name: "IX_MobilePushToPriceGroup_MobilePushByPriceGroupId",
                table: "MobilePushToPriceGroup",
                column: "MobilePushByPriceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_IsSoftDeleted",
                table: "OrderItems",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuItemId",
                table: "OrderItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientAccountId",
                table: "Orders",
                column: "ClientAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatorWorkerAccountId",
                table: "Orders",
                column: "CreatorWorkerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryAddressId",
                table: "Orders",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IsSoftDeleted",
                table: "Orders",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestaurantId",
                table: "Orders",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupStops_IsSoftDeleted",
                table: "PickupStops",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PickupStops_IssuerId",
                table: "PickupStops",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupStops_RestaurantId",
                table: "PickupStops",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupTimeOpenCloses_IsSoftDeleted",
                table: "PickupTimeOpenCloses",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PickupTimeOpenCloses_RestaurantId",
                table: "PickupTimeOpenCloses",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceGroups_IsSoftDeleted",
                table: "PriceGroups",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantLatLngs_IsSoftDeleted",
                table: "RestaurantLatLngs",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantLatLngs_RestaurantId",
                table: "RestaurantLatLngs",
                column: "RestaurantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_CityId",
                table: "Restaurants",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_IsSoftDeleted",
                table: "Restaurants",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TokenSessions_IsSoftDeleted",
                table: "TokenSessions",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TokenSessions_Token",
                table: "TokenSessions",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_TokenSessions_WorkerAccountId",
                table: "TokenSessions",
                column: "WorkerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_VacanciesDatas_IsSoftDeleted",
                table: "VacanciesDatas",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_VkUrlDatas_IsSoftDeleted",
                table: "VkUrlDatas",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerAccounts_IsSoftDeleted",
                table: "WorkerAccounts",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerAccountToRestaurant_RestaurantId",
                table: "WorkerAccountToRestaurant",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerAccountToRole_WorkerRoleId",
                table: "WorkerAccountToRole",
                column: "WorkerRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerRoles_IsSoftDeleted",
                table: "WorkerRoles",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneLatLngs_IsSoftDeleted",
                table: "ZoneLatLngs",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneLatLngs_RestaurantId",
                table: "ZoneLatLngs",
                column: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutDatas");

            migrationBuilder.DropTable(
                name: "ApplicationStartupImageDatas");

            migrationBuilder.DropTable(
                name: "ApplicationTerminationDatas");

            migrationBuilder.DropTable(
                name: "BannerToCity");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "CityLatLngs");

            migrationBuilder.DropTable(
                name: "ClientLoginRequests");

            migrationBuilder.DropTable(
                name: "DeliveryAddressLatLngs");

            migrationBuilder.DropTable(
                name: "DeliveryStops");

            migrationBuilder.DropTable(
                name: "DeliveryTermsDatas");

            migrationBuilder.DropTable(
                name: "DeliveryTimeOpenCloses");

            migrationBuilder.DropTable(
                name: "FavoriteItems");

            migrationBuilder.DropTable(
                name: "InstagramUrlDatas");

            migrationBuilder.DropTable(
                name: "MenuCPFCs");

            migrationBuilder.DropTable(
                name: "MenuItemMeasure");

            migrationBuilder.DropTable(
                name: "MenuItemToMenuProduct");

            migrationBuilder.DropTable(
                name: "MenuItemToPriceGroup");

            migrationBuilder.DropTable(
                name: "MobilePushToCity");

            migrationBuilder.DropTable(
                name: "MobilePushToPriceGroup");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PickupStops");

            migrationBuilder.DropTable(
                name: "PickupTimeOpenCloses");

            migrationBuilder.DropTable(
                name: "RestaurantLatLngs");

            migrationBuilder.DropTable(
                name: "TokenSessions");

            migrationBuilder.DropTable(
                name: "VacanciesDatas");

            migrationBuilder.DropTable(
                name: "VkUrlDatas");

            migrationBuilder.DropTable(
                name: "WorkerAccountToRestaurant");

            migrationBuilder.DropTable(
                name: "WorkerAccountToRole");

            migrationBuilder.DropTable(
                name: "ZoneLatLngs");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "MenuProducts");

            migrationBuilder.DropTable(
                name: "MobileNotificationsByCity");

            migrationBuilder.DropTable(
                name: "MobileNotificationsByPriceGroup");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "WorkerRoles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "DeliveryAddresses");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "WorkerAccounts");

            migrationBuilder.DropTable(
                name: "ClientAccounts");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "PriceGroups");
        }
    }
}
