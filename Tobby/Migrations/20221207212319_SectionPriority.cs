using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tobby.Migrations
{
    public partial class SectionPriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectionPriority",
                table: "Element",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectionPriority",
                table: "Element");
        }
    }
}
