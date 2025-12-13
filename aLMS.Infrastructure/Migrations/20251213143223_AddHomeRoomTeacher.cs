using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeRoomTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    RefreshToken = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PermissionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    RoleName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "school",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_school", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role_permission",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_role_permission_permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permission_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_school_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "school",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "behaviour",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Video = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Result = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_behaviour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_behaviour_user_StudentId",
                        column: x => x.StudentId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_behaviour_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "department",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    DepartmentName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    HeadId = table.Column<Guid>(type: "uuid", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_department_school_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "school",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_department_user_HeadId",
                        column: x => x.HeadId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "parent_profile",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parent_profile", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_parent_profile_user_StudentId",
                        column: x => x.StudentId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_parent_profile_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teacher_profile",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    HireDate = table.Column<DateTime>(type: "date", nullable: false),
                    Specialization = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_profile", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_teacher_profile_department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teacher_profile_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ClassName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Grade = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SchoolYear = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    HomeroomTeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class", x => x.Id);
                    table.ForeignKey(
                        name: "FK_class_teacher_profile_HomeroomTeacherId",
                        column: x => x.HomeroomTeacherId,
                        principalTable: "teacher_profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "student_profile",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollDate = table.Column<DateTime>(type: "date", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_profile", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_student_profile_class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_profile_school_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "school",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_profile_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_subject_class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DateFrom = table.Column<DateTime>(type: "date", nullable: false),
                    DateTo = table.Column<DateTime>(type: "date", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_topic_subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lesson",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ResourceType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lesson_topic_TopicId",
                        column: x => x.TopicId,
                        principalTable: "topic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ExerciseFile = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    HasTimeLimit = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TimeLimit = table.Column<int>(type: "integer", nullable: true),
                    QuestionLayout = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    OrderNumber = table.Column<int>(type: "integer", nullable: false),
                    TotalScore = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exercise_lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "lesson",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_exercise_topic_TopicId",
                        column: x => x.TopicId,
                        principalTable: "topic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionContent = table.Column<string>(type: "text", nullable: false),
                    QuestionImage = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    QuestionType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    OrderNumber = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Explanation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_question_exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_exercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Score = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_exercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_exercise_exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_exercise_user_StudentId",
                        column: x => x.StudentId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_exercise_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerContent = table.Column<string>(type: "text", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    OrderNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_answer_question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    StudentExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerText = table.Column<string>(type: "text", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_answer_answer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_student_answer_question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_answer_student_exercise_StudentExerciseId",
                        column: x => x.StudentExerciseId,
                        principalTable: "student_exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Username",
                table: "account",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_answer_QuestionId",
                table: "answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_behaviour_StudentId",
                table: "behaviour",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_behaviour_UserId",
                table: "behaviour",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_class_HomeroomTeacherId",
                table: "class",
                column: "HomeroomTeacherId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_department_HeadId",
                table: "department",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_department_SchoolId",
                table: "department",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_LessonId",
                table: "exercise",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_TopicId",
                table: "exercise",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_TopicId",
                table: "lesson",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_parent_profile_StudentId",
                table: "parent_profile",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_PermissionName",
                table: "permission",
                column: "PermissionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_question_ExerciseId",
                table: "question",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_role_RoleName",
                table: "role",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_PermissionId",
                table: "role_permission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_AnswerId",
                table: "student_answer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_QuestionId",
                table: "student_answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_StudentExerciseId",
                table: "student_answer",
                column: "StudentExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_student_exercise_ExerciseId",
                table: "student_exercise",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_student_exercise_StudentId",
                table: "student_exercise",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_student_exercise_UserId",
                table: "student_exercise",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_student_profile_ClassId",
                table: "student_profile",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_student_profile_SchoolId",
                table: "student_profile",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_subject_ClassId",
                table: "subject",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_profile_DepartmentId",
                table: "teacher_profile",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_topic_SubjectId",
                table: "topic",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_user_AccountId",
                table: "user",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_Email",
                table: "user",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_RoleId",
                table: "user",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_user_SchoolId",
                table: "user",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "behaviour");

            migrationBuilder.DropTable(
                name: "parent_profile");

            migrationBuilder.DropTable(
                name: "role_permission");

            migrationBuilder.DropTable(
                name: "student_answer");

            migrationBuilder.DropTable(
                name: "student_profile");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "answer");

            migrationBuilder.DropTable(
                name: "student_exercise");

            migrationBuilder.DropTable(
                name: "question");

            migrationBuilder.DropTable(
                name: "exercise");

            migrationBuilder.DropTable(
                name: "lesson");

            migrationBuilder.DropTable(
                name: "topic");

            migrationBuilder.DropTable(
                name: "subject");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "teacher_profile");

            migrationBuilder.DropTable(
                name: "department");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "school");
        }
    }
}
