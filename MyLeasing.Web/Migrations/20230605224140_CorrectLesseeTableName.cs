using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLeasing.Web.Migrations
{
    public partial class CorrectLesseeTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessee_AspNetUsers_UserId",
                table: "Lessee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lessee",
                table: "Lessee");

            migrationBuilder.RenameTable(
                name: "Lessee",
                newName: "Lessees");

            migrationBuilder.RenameIndex(
                name: "IX_Lessee_UserId",
                table: "Lessees",
                newName: "IX_Lessees_UserId");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Lessees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lessees",
                table: "Lessees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessees_AspNetUsers_UserId",
                table: "Lessees",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessees_AspNetUsers_UserId",
                table: "Lessees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lessees",
                table: "Lessees");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Lessees");

            migrationBuilder.RenameTable(
                name: "Lessees",
                newName: "Lessee");

            migrationBuilder.RenameIndex(
                name: "IX_Lessees_UserId",
                table: "Lessee",
                newName: "IX_Lessee_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lessee",
                table: "Lessee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessee_AspNetUsers_UserId",
                table: "Lessee",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
