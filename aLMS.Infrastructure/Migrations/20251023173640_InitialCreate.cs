using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: true)
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
                    PermissionId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    RoleId1 = table.Column<Guid>(type: "uuid", nullable: true),
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
                        name: "FK_role_permission_permission_PermissionId1",
                        column: x => x.PermissionId1,
                        principalTable: "permission",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_role_permission_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permission_role_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "grade",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    GradeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SchoolYear = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_grade_school_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "school",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grade_school_SchoolId1",
                        column: x => x.SchoolId1,
                        principalTable: "school",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "class",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ClassName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    GradeId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradeId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class", x => x.Id);
                    table.ForeignKey(
                        name: "FK_class_grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "grade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_grade_GradeId1",
                        column: x => x.GradeId1,
                        principalTable: "grade",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "subject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId1 = table.Column<Guid>(type: "uuid", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_subject_class_ClassId1",
                        column: x => x.ClassId1,
                        principalTable: "class",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "topic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DateFrom = table.Column<DateTime>(type: "date", nullable: false),
                    DateTo = table.Column<DateTime>(type: "date", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId1 = table.Column<Guid>(type: "uuid", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_topic_subject_SubjectId1",
                        column: x => x.SubjectId1,
                        principalTable: "subject",
                        principalColumn: "Id");
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
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId1 = table.Column<Guid>(type: "uuid", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_lesson_topic_TopicId1",
                        column: x => x.TopicId1,
                        principalTable: "topic",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "exercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ExerciseFile = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    HasTimeLimit = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TimeLimit = table.Column<int>(type: "integer", nullable: true),
                    QuestionLayout = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    OrderNumber = table.Column<int>(type: "integer", nullable: false),
                    TotalScore = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exercise_lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exercise_lesson_LessonId1",
                        column: x => x.LessonId1,
                        principalTable: "lesson",
                        principalColumn: "Id");
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
                    Explanation = table.Column<string>(type: "text", nullable: false),
                    ExerciseId1 = table.Column<Guid>(type: "uuid", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_question_exercise_ExerciseId1",
                        column: x => x.ExerciseId1,
                        principalTable: "exercise",
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
                    OrderNumber = table.Column<int>(type: "integer", nullable: false),
                    QuestionId1 = table.Column<Guid>(type: "uuid", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_answer_question_QuestionId1",
                        column: x => x.QuestionId1,
                        principalTable: "question",
                        principalColumn: "Id");
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
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_behaviour", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "department",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    DepartmentName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    HeadId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department", x => x.Id);
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
                    SubmittedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    AnswerId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    QuestionId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentExerciseId1 = table.Column<Guid>(type: "uuid", nullable: true)
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
                        name: "FK_student_answer_answer_AnswerId1",
                        column: x => x.AnswerId1,
                        principalTable: "answer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_student_answer_question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_answer_question_QuestionId1",
                        column: x => x.QuestionId1,
                        principalTable: "question",
                        principalColumn: "Id");
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
                    ExerciseId1 = table.Column<Guid>(type: "uuid", nullable: true),
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
                        name: "FK_student_exercise_exercise_ExerciseId1",
                        column: x => x.ExerciseId1,
                        principalTable: "exercise",
                        principalColumn: "Id");
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
                });

            migrationBuilder.CreateTable(
                name: "teacher_profile",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    HireDate = table.Column<DateTime>(type: "date", nullable: false),
                    Specialization = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DepartmentId1 = table.Column<Guid>(type: "uuid", nullable: true),
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
                        name: "FK_teacher_profile_department_DepartmentId1",
                        column: x => x.DepartmentId1,
                        principalTable: "department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_teacher_profile_school_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "school",
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
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentProfileUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherProfileUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentProfileUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    SchoolId1 = table.Column<Guid>(type: "uuid", nullable: true)
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
                        name: "FK_user_parent_profile_ParentProfileUserId",
                        column: x => x.ParentProfileUserId,
                        principalTable: "parent_profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "role",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_user_school_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "school",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_school_SchoolId1",
                        column: x => x.SchoolId1,
                        principalTable: "school",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_user_student_profile_StudentProfileUserId",
                        column: x => x.StudentProfileUserId,
                        principalTable: "student_profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_teacher_profile_TeacherProfileUserId",
                        column: x => x.TeacherProfileUserId,
                        principalTable: "teacher_profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_Username",
                table: "account",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_answer_QuestionId",
                table: "answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_answer_QuestionId1",
                table: "answer",
                column: "QuestionId1");

            migrationBuilder.CreateIndex(
                name: "IX_behaviour_StudentId",
                table: "behaviour",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_behaviour_UserId",
                table: "behaviour",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_class_GradeId",
                table: "class",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_class_GradeId1",
                table: "class",
                column: "GradeId1");

            migrationBuilder.CreateIndex(
                name: "IX_department_HeadId",
                table: "department",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_LessonId",
                table: "exercise",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_LessonId1",
                table: "exercise",
                column: "LessonId1");

            migrationBuilder.CreateIndex(
                name: "IX_grade_SchoolId",
                table: "grade",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_grade_SchoolId1",
                table: "grade",
                column: "SchoolId1");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_TopicId",
                table: "lesson",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_TopicId1",
                table: "lesson",
                column: "TopicId1");

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
                name: "IX_question_ExerciseId1",
                table: "question",
                column: "ExerciseId1");

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
                name: "IX_role_permission_PermissionId1",
                table: "role_permission",
                column: "PermissionId1");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_RoleId1",
                table: "role_permission",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_AnswerId",
                table: "student_answer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_AnswerId1",
                table: "student_answer",
                column: "AnswerId1");

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_QuestionId",
                table: "student_answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_QuestionId1",
                table: "student_answer",
                column: "QuestionId1");

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_StudentExerciseId",
                table: "student_answer",
                column: "StudentExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_student_answer_StudentExerciseId1",
                table: "student_answer",
                column: "StudentExerciseId1");

            migrationBuilder.CreateIndex(
                name: "IX_student_exercise_ExerciseId",
                table: "student_exercise",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_student_exercise_ExerciseId1",
                table: "student_exercise",
                column: "ExerciseId1");

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
                name: "IX_subject_ClassId1",
                table: "subject",
                column: "ClassId1");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_profile_DepartmentId",
                table: "teacher_profile",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_profile_DepartmentId1",
                table: "teacher_profile",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_profile_SchoolId",
                table: "teacher_profile",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_topic_SubjectId",
                table: "topic",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_topic_SubjectId1",
                table: "topic",
                column: "SubjectId1");

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
                name: "IX_user_ParentProfileUserId",
                table: "user",
                column: "ParentProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_RoleId",
                table: "user",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_user_RoleId1",
                table: "user",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_user_SchoolId",
                table: "user",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_user_SchoolId1",
                table: "user",
                column: "SchoolId1");

            migrationBuilder.CreateIndex(
                name: "IX_user_StudentProfileUserId",
                table: "user",
                column: "StudentProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_TeacherProfileUserId",
                table: "user",
                column: "TeacherProfileUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_behaviour_user_StudentId",
                table: "behaviour",
                column: "StudentId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_behaviour_user_UserId",
                table: "behaviour",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_department_user_HeadId",
                table: "department",
                column: "HeadId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_parent_profile_user_StudentId",
                table: "parent_profile",
                column: "StudentId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_parent_profile_user_UserId",
                table: "parent_profile",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_answer_student_exercise_StudentExerciseId",
                table: "student_answer",
                column: "StudentExerciseId",
                principalTable: "student_exercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_answer_student_exercise_StudentExerciseId1",
                table: "student_answer",
                column: "StudentExerciseId1",
                principalTable: "student_exercise",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_student_exercise_user_StudentId",
                table: "student_exercise",
                column: "StudentId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_exercise_user_UserId",
                table: "student_exercise",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_student_profile_user_UserId",
                table: "student_profile",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teacher_profile_user_UserId",
                table: "teacher_profile",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_department_user_HeadId",
                table: "department");

            migrationBuilder.DropForeignKey(
                name: "FK_parent_profile_user_StudentId",
                table: "parent_profile");

            migrationBuilder.DropForeignKey(
                name: "FK_parent_profile_user_UserId",
                table: "parent_profile");

            migrationBuilder.DropForeignKey(
                name: "FK_student_profile_user_UserId",
                table: "student_profile");

            migrationBuilder.DropForeignKey(
                name: "FK_teacher_profile_user_UserId",
                table: "teacher_profile");

            migrationBuilder.DropTable(
                name: "behaviour");

            migrationBuilder.DropTable(
                name: "role_permission");

            migrationBuilder.DropTable(
                name: "student_answer");

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
                name: "user");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "parent_profile");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "student_profile");

            migrationBuilder.DropTable(
                name: "teacher_profile");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "department");

            migrationBuilder.DropTable(
                name: "grade");

            migrationBuilder.DropTable(
                name: "school");
        }
    }
}
