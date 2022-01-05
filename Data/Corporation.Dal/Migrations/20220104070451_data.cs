using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporation.Dal.Migrations
{
    public partial class data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.CreateTable(
                name: "Com1Autoclaves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Com1Autoclaves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Com1Packs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Com1Packs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Com1ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Units = table.Column<int>(type: "INTEGER", nullable: false),
                    Com1Loose1RawId = table.Column<int>(type: "INTEGER", nullable: false),
                    Com1Loose1RawValue = table.Column<double>(type: "REAL", nullable: false),
                    Com1Loose2RawId = table.Column<int>(type: "INTEGER", nullable: false),
                    Com1Loose2RawValue = table.Column<double>(type: "REAL", nullable: false),
                    IsDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Com1ProductTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Com1ProductTypes_Com1LooseRaws_Com1Loose1RawId",
                        column: x => x.Com1Loose1RawId,
                        principalTable: "Com1LooseRaws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1ProductTypes_Com1LooseRaws_Com1Loose2RawId",
                        column: x => x.Com1Loose2RawId,
                        principalTable: "Com1LooseRaws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Com1Autoclaves1ShiftDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Com1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Com1AutoclaveId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AutoclavedTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    AutoclavedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Com1Autoclaves1ShiftDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Com1Autoclaves1ShiftDatas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Autoclaves1ShiftDatas_Com1Autoclaves_Com1AutoclaveId",
                        column: x => x.Com1AutoclaveId,
                        principalTable: "Com1Autoclaves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Autoclaves1ShiftDatas_Com1Shifts_Com1ShiftId",
                        column: x => x.Com1ShiftId,
                        principalTable: "Com1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Com1Packing1ShiftDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Com1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Com1PackId = table.Column<int>(type: "INTEGER", nullable: false),
                    Com1PackCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Com1Packing1ShiftDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Com1Packing1ShiftDatas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Packing1ShiftDatas_Com1Packs_Com1PackId",
                        column: x => x.Com1PackId,
                        principalTable: "Com1Packs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Packing1ShiftDatas_Com1Shifts_Com1ShiftId",
                        column: x => x.Com1ShiftId,
                        principalTable: "Com1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Com1Warehouse2ShiftDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Com1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Com1PackId = table.Column<int>(type: "INTEGER", nullable: false),
                    Com1PackValue = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Com1Warehouse2ShiftDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Com1Warehouse2ShiftDatas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Warehouse2ShiftDatas_Com1Packs_Com1PackId",
                        column: x => x.Com1PackId,
                        principalTable: "Com1Packs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Warehouse2ShiftDatas_Com1Shifts_Com1ShiftId",
                        column: x => x.Com1ShiftId,
                        principalTable: "Com1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Com1Press1ShiftDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Com1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Com1ProductTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Com1ProductTypeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Com1Loose1RawValue = table.Column<double>(type: "REAL", nullable: false),
                    Com1Loose2RawValue = table.Column<double>(type: "REAL", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Com1Press1ShiftDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Com1Press1ShiftDatas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Press1ShiftDatas_Com1ProductTypes_Com1ProductTypeId",
                        column: x => x.Com1ProductTypeId,
                        principalTable: "Com1ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Press1ShiftDatas_Com1Shifts_Com1ShiftId",
                        column: x => x.Com1ShiftId,
                        principalTable: "Com1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Com1Autoclaves1ShiftDatas_Com1AutoclaveId",
                table: "Com1Autoclaves1ShiftDatas",
                column: "Com1AutoclaveId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Autoclaves1ShiftDatas_Com1ShiftId",
                table: "Com1Autoclaves1ShiftDatas",
                column: "Com1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Autoclaves1ShiftDatas_UserId",
                table: "Com1Autoclaves1ShiftDatas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Packing1ShiftDatas_Com1PackId",
                table: "Com1Packing1ShiftDatas",
                column: "Com1PackId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Packing1ShiftDatas_Com1ShiftId",
                table: "Com1Packing1ShiftDatas",
                column: "Com1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Packing1ShiftDatas_UserId",
                table: "Com1Packing1ShiftDatas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Press1ShiftDatas_Com1ProductTypeId",
                table: "Com1Press1ShiftDatas",
                column: "Com1ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Press1ShiftDatas_Com1ShiftId",
                table: "Com1Press1ShiftDatas",
                column: "Com1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Press1ShiftDatas_UserId",
                table: "Com1Press1ShiftDatas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1ProductTypes_Com1Loose1RawId",
                table: "Com1ProductTypes",
                column: "Com1Loose1RawId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1ProductTypes_Com1Loose2RawId",
                table: "Com1ProductTypes",
                column: "Com1Loose2RawId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Warehouse2ShiftDatas_Com1PackId",
                table: "Com1Warehouse2ShiftDatas",
                column: "Com1PackId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Warehouse2ShiftDatas_Com1ShiftId",
                table: "Com1Warehouse2ShiftDatas",
                column: "Com1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Warehouse2ShiftDatas_UserId",
                table: "Com1Warehouse2ShiftDatas",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Com1Autoclaves1ShiftDatas");

            migrationBuilder.DropTable(
                name: "Com1Packing1ShiftDatas");

            migrationBuilder.DropTable(
                name: "Com1Press1ShiftDatas");

            migrationBuilder.DropTable(
                name: "Com1Warehouse2ShiftDatas");

            migrationBuilder.DropTable(
                name: "Com1Autoclaves");

            migrationBuilder.DropTable(
                name: "Com1ProductTypes");

            migrationBuilder.DropTable(
                name: "Com1Packs");

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    Units = table.Column<int>(type: "INTEGER", nullable: false),
                    Volume = table.Column<double>(type: "REAL", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });
        }
    }
}
