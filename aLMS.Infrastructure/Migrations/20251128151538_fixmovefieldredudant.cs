using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixmovefieldredudant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_answer_student_exercise_StudentExerciseId1",
                table: "student_answer");

            migrationBuilder.DropIndex(
                name: "IX_student_answer_StudentExerciseId1",
                table: "student_answer");

            migrationBuilder.DropColumn(
                name: "StudentExerciseId1",
                table: "student_answer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentExerciseId1",
                table: "student_answer",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_StudentExerciseId1",
                table: "student_answer",
                column: "StudentExerciseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_student_answer_student_exercise_StudentExerciseId1",
                table: "student_answer",
                column: "StudentExerciseId1",
                principalTable: "student_exercise",
                principalColumn: "Id");
        }
    }
}
