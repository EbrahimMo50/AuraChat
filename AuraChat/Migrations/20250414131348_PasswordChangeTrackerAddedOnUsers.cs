using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuraChat.Migrations
{
    /// <inheritdoc />
    public partial class PasswordChangeTrackerAddedOnUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserSettings_PasswordChangeCounter",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSettings_PasswordChangeCounter",
                table: "Users");
        }
    }
}
