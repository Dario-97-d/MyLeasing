using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLeasing.Web.Migrations
{
    public partial class ChangePhotoUrlToPhotoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Lessees");

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "Owners",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "Lessees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Lessees");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Lessees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
