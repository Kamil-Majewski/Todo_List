using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo_List.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedstartdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) 
        {
            migrationBuilder.DropColumn(
                name: "RecurringCommitment_StartDate",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Tasks",
                newName: "RecurrenceStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecurrenceStart",
                table: "Tasks",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "RecurringCommitment_StartDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true);
        }
    }
}
