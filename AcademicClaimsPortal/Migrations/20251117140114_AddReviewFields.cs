using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicClaimsPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewDate",
                table: "Claims",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewedBy",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewDate",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "Claims");
        }
    }
}
