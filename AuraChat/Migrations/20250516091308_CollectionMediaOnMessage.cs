using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuraChat.Migrations
{
    /// <inheritdoc />
    public partial class CollectionMediaOnMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MediaGuid",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MediaGuid",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
