using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilgeShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "IsDeleted", "LastName", "ModifiedDate", "Password", "UserType" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@bilgeshop.com", "Bilge", false, "Adam", null, "CfDJ8JQjfQ9e7tVOuvsvkK21ZW502E3s3qGoM5sVCfgwfmIaZ68v8GEu1jVtfvRrxFET6-xCl_IZjZnz3xxaAfRxjYGOrELmu1xbobPq5mJIqJr2M4W_YlnOjCcGEjTdwne5vA", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
