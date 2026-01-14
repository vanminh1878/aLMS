using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_ClassSubject_PK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subject_class_ClassId",
                table: "subject");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                table: "subject",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "class_subject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolYear = table.Column<string>(type: "varchar(20)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubjectId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_subject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_class_subject_class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_subject_subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_subject_subject_SubjectId1",
                        column: x => x.SubjectId1,
                        principalTable: "subject",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "timetable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    DayOfWeek = table.Column<short>(type: "smallint", nullable: false),
                    PeriodNumber = table.Column<short>(type: "smallint", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Room = table.Column<string>(type: "varchar(50)", nullable: true),
                    SchoolYear = table.Column<string>(type: "varchar(20)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timetable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_timetable_class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_timetable_subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_timetable_teacher_profile_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "teacher_profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class_subject_teacher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ClassSubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolYear = table.Column<string>(type: "varchar(20)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_subject_teacher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_class_subject_teacher_class_subject_ClassSubjectId",
                        column: x => x.ClassSubjectId,
                        principalTable: "class_subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_subject_teacher_teacher_profile_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "teacher_profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_class_subject_ClassId_SubjectId",
                table: "class_subject",
                columns: new[] { "ClassId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_class_subject_SubjectId",
                table: "class_subject",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_class_subject_SubjectId1",
                table: "class_subject",
                column: "SubjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_class_subject_teacher_ClassSubjectId",
                table: "class_subject_teacher",
                column: "ClassSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_class_subject_teacher_TeacherId",
                table: "class_subject_teacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_ClassId",
                table: "timetable",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_SubjectId",
                table: "timetable",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_timetable_TeacherId",
                table: "timetable",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_subject_class_ClassId",
                table: "subject",
                column: "ClassId",
                principalTable: "class",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subject_class_ClassId",
                table: "subject");

            migrationBuilder.DropTable(
                name: "class_subject_teacher");

            migrationBuilder.DropTable(
                name: "timetable");

            migrationBuilder.DropTable(
                name: "class_subject");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                table: "subject",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_subject_class_ClassId",
                table: "subject",
                column: "ClassId",
                principalTable: "class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
