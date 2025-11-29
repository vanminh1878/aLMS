using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixUserColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_parent_profile_ParentProfileUserId",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_user_student_profile_StudentProfileUserId",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_user_teacher_profile_TeacherProfileUserId",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_ParentProfileUserId",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_StudentProfileUserId",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_TeacherProfileUserId",
                table: "user");

            migrationBuilder.DropColumn(
                name: "ParentProfileUserId",
                table: "user");

            migrationBuilder.DropColumn(
                name: "StudentProfileUserId",
                table: "user");

            migrationBuilder.DropColumn(
                name: "TeacherProfileUserId",
                table: "user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentProfileUserId",
                table: "user",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StudentProfileUserId",
                table: "user",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherProfileUserId",
                table: "user",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_user_ParentProfileUserId",
                table: "user",
                column: "ParentProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_StudentProfileUserId",
                table: "user",
                column: "StudentProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_TeacherProfileUserId",
                table: "user",
                column: "TeacherProfileUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_parent_profile_ParentProfileUserId",
                table: "user",
                column: "ParentProfileUserId",
                principalTable: "parent_profile",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_student_profile_StudentProfileUserId",
                table: "user",
                column: "StudentProfileUserId",
                principalTable: "student_profile",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_teacher_profile_TeacherProfileUserId",
                table: "user",
                column: "TeacherProfileUserId",
                principalTable: "teacher_profile",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
