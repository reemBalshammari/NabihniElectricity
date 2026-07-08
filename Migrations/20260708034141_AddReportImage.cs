using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NabihniElectricity.Migrations
{
    /// <inheritdoc />
    public partial class AddReportImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Reports");
        }
    }
}
