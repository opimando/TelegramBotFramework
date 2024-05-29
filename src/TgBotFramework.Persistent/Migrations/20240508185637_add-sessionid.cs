using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgBotFramework.Persistent.Migrations
{
    /// <inheritdoc />
    public partial class addsessionid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SessionId",
                table: "States",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "States");
        }
    }
}
