using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSchoolIdInDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "department",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_department_SchoolId",
                table: "department",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_department_school_SchoolId",
                table: "department",
                column: "SchoolId",
                principalTable: "school",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_department_school_SchoolId",
                table: "department");

            migrationBuilder.DropIndex(
                name: "IX_department_SchoolId",
                table: "department");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "department");
        }
    }
}
