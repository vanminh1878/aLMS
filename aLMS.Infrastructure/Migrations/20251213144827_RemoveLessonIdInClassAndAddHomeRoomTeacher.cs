using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLessonIdInClassAndAddHomeRoomTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exercise_lesson_LessonId",
                table: "exercise");

            migrationBuilder.DropIndex(
                name: "IX_exercise_LessonId",
                table: "exercise");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "exercise");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LessonId",
                table: "exercise",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_exercise_LessonId",
                table: "exercise",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_exercise_lesson_LessonId",
                table: "exercise",
                column: "LessonId",
                principalTable: "lesson",
                principalColumn: "Id");
        }
    }
}
