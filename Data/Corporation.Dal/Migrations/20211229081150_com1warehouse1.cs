using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporation.Dal.Migrations
{
    public partial class com1warehouse1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Com1LooseRaws",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Com1LooseRaws", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Com1Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Com1Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Com1Warehouse1ShiftDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Com1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Com1Tank1LooseRawId = table.Column<int>(type: "INTEGER", nullable: false),
                    Com1Tank1LooseRawValue = table.Column<double>(type: "REAL", nullable: false),
                    Com1Tank2LooseRawId = table.Column<int>(type: "INTEGER", nullable: false),
                    Com1Tank2LooseRawValue = table.Column<double>(type: "REAL", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Com1Warehouse1ShiftDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Com1Warehouse1ShiftDatas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Warehouse1ShiftDatas_Com1LooseRaws_Com1Tank1LooseRawId",
                        column: x => x.Com1Tank1LooseRawId,
                        principalTable: "Com1LooseRaws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Warehouse1ShiftDatas_Com1LooseRaws_Com1Tank2LooseRawId",
                        column: x => x.Com1Tank2LooseRawId,
                        principalTable: "Com1LooseRaws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Com1Warehouse1ShiftDatas_Com1Shifts_Com1ShiftId",
                        column: x => x.Com1ShiftId,
                        principalTable: "Com1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Com1Warehouse1ShiftDatas_Com1ShiftId",
                table: "Com1Warehouse1ShiftDatas",
                column: "Com1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Warehouse1ShiftDatas_Com1Tank1LooseRawId",
                table: "Com1Warehouse1ShiftDatas",
                column: "Com1Tank1LooseRawId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Warehouse1ShiftDatas_Com1Tank2LooseRawId",
                table: "Com1Warehouse1ShiftDatas",
                column: "Com1Tank2LooseRawId");

            migrationBuilder.CreateIndex(
                name: "IX_Com1Warehouse1ShiftDatas_UserId",
                table: "Com1Warehouse1ShiftDatas",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Com1Warehouse1ShiftDatas");

            migrationBuilder.DropTable(
                name: "Com1LooseRaws");

            migrationBuilder.DropTable(
                name: "Com1Shifts");
        }
    }
}
