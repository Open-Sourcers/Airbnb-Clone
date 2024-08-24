using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airbnb.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Properties_Owner",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_Owner",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AppUserId",
                table: "Properties",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AspNetUsers_AppUserId",
                table: "Properties",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AspNetUsers_AppUserId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_AppUserId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Properties");

            migrationBuilder.AddColumn<int>(
                name: "Owner",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_Owner",
                table: "Images",
                column: "Owner");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Properties_Owner",
                table: "Images",
                column: "Owner",
                principalTable: "Properties",
                principalColumn: "Id");
        }
    }
}
