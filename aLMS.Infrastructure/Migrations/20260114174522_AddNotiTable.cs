using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "exercise",
                type: "varchar(30)",
                nullable: false,
                defaultValue: "practice");

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "varchar(150)", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    TargetType = table.Column<string>(type: "varchar(30)", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notification_user_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "quality",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quality", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "student_evaluation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    Semester = table.Column<string>(type: "varchar(10)", nullable: false),
                    SchoolYear = table.Column<string>(type: "varchar(20)", nullable: false),
                    FinalScore = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Level = table.Column<string>(type: "varchar(20)", nullable: false),
                    GeneralComment = table.Column<string>(type: "text", nullable: false),
                    FinalEvaluation = table.Column<string>(type: "varchar(20)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_evaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_evaluation_class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_evaluation_user_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_evaluation_user_StudentId",
                        column: x => x.StudentId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "virtual_classroom",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    TimetableId = table.Column<Guid>(type: "uuid", nullable: true),
                    DayOfWeek = table.Column<short>(type: "smallint", nullable: true),
                    PeriodNumber = table.Column<short>(type: "smallint", nullable: true),
                    Title = table.Column<string>(type: "varchar(150)", nullable: false),
                    MeetingUrl = table.Column<string>(type: "varchar(500)", nullable: false),
                    MeetingId = table.Column<string>(type: "varchar(100)", nullable: true),
                    Password = table.Column<string>(type: "varchar(50)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_virtual_classroom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_virtual_classroom_class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_virtual_classroom_subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_virtual_classroom_teacher_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "teacher_profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_virtual_classroom_timetable_TimetableId",
                        column: x => x.TimetableId,
                        principalTable: "timetable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "student_quality_evaluation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    StudentEvaluationId = table.Column<Guid>(type: "uuid", nullable: false),
                    QualityId = table.Column<Guid>(type: "uuid", nullable: false),
                    QualityLevel = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_quality_evaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_quality_evaluation_quality_QualityId",
                        column: x => x.QualityId,
                        principalTable: "quality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_quality_evaluation_student_evaluation_StudentEvalua~",
                        column: x => x.StudentEvaluationId,
                        principalTable: "student_evaluation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_subject_comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    StudentEvaluationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_subject_comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_subject_comment_student_evaluation_StudentEvaluatio~",
                        column: x => x.StudentEvaluationId,
                        principalTable: "student_evaluation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_subject_comment_subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notification_CreatedBy",
                table: "notification",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_student_evaluation_ClassId",
                table: "student_evaluation",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_student_evaluation_CreatedBy",
                table: "student_evaluation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_student_evaluation_StudentId",
                table: "student_evaluation",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_student_quality_evaluation_QualityId",
                table: "student_quality_evaluation",
                column: "QualityId");

            migrationBuilder.CreateIndex(
                name: "IX_student_quality_evaluation_StudentEvaluationId",
                table: "student_quality_evaluation",
                column: "StudentEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_student_subject_comment_StudentEvaluationId",
                table: "student_subject_comment",
                column: "StudentEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_student_subject_comment_SubjectId",
                table: "student_subject_comment",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_classroom_ClassId",
                table: "virtual_classroom",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_classroom_CreatedBy",
                table: "virtual_classroom",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_classroom_SubjectId",
                table: "virtual_classroom",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_virtual_classroom_TimetableId",
                table: "virtual_classroom",
                column: "TimetableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "student_quality_evaluation");

            migrationBuilder.DropTable(
                name: "student_subject_comment");

            migrationBuilder.DropTable(
                name: "virtual_classroom");

            migrationBuilder.DropTable(
                name: "quality");

            migrationBuilder.DropTable(
                name: "student_evaluation");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "exercise");
        }
    }
}
