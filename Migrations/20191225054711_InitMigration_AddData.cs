using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core3.xWebApi.Migrations
{
    public partial class InitMigration_AddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("99ba5433-df5f-a898-c8e0-78b8ba55f251"), "OK", "Ali" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("f32a94e2-dca3-767e-7ce0-06ccae6ef474"), "OK", "TX" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Introduction", "Name" },
                values: new object[] { new Guid("19660152-e925-1bfc-cb2c-b8eba0fbca82"), "OK", "JD" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("19660152-e925-1bfc-cb2c-b8eba0fbca82"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("99ba5433-df5f-a898-c8e0-78b8ba55f251"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("f32a94e2-dca3-767e-7ce0-06ccae6ef474"));
        }
    }
}
