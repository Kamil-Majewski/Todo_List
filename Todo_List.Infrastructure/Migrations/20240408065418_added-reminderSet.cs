using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo_List.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedreminderSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReminderSet",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderSet",
                table: "Tasks");
        }
    }
}
