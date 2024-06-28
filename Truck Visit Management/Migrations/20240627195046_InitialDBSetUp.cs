using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Truck_Visit_Management.Migrations
{
    /// <inheritdoc />
    public partial class InitialDBSetUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitRecordEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TruckLicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitRecordEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisitRecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityEntity_VisitRecordEntity_VisitRecordId",
                        column: x => x.VisitRecordId,
                        principalTable: "VisitRecordEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverInformationEntity",
                columns: table => new
                {
                    VisitRecordEntityId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverInformationEntity", x => x.VisitRecordEntityId);
                    table.ForeignKey(
                        name: "FK_DriverInformationEntity_VisitRecordEntity_VisitRecordEntityId",
                        column: x => x.VisitRecordEntityId,
                        principalTable: "VisitRecordEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityEntity_VisitRecordId",
                table: "ActivityEntity",
                column: "VisitRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityEntity");

            migrationBuilder.DropTable(
                name: "DriverInformationEntity");

            migrationBuilder.DropTable(
                name: "VisitRecordEntity");
        }
    }
}
