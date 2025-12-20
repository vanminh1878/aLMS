using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentClassEnroll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_profile_class_ClassId",
                table: "student_profile");

            migrationBuilder.DropIndex(
                name: "IX_student_profile_ClassId",
                table: "student_profile");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "student_profile");

            migrationBuilder.CreateTable(
                name: "student_class_enrollment",
                columns: table => new
                {
                    StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_class_enrollment", x => new { x.StudentProfileId, x.ClassId });
                    table.ForeignKey(
                        name: "FK_student_class_enrollment_class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_class_enrollment_student_profile_StudentProfileId",
                        column: x => x.StudentProfileId,
                        principalTable: "student_profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_class_enrollment_ClassId",
                table: "student_class_enrollment",
                column: "ClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_class_enrollment");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "student_profile",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_student_profile_ClassId",
                table: "student_profile",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_student_profile_class_ClassId",
                table: "student_profile",
                column: "ClassId",
                principalTable: "class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
