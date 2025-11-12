using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeyConflicts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answer_question_QuestionId1",
                table: "answer");

            migrationBuilder.DropForeignKey(
                name: "FK_user_account_AccountId",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_answer_QuestionId1",
                table: "answer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_account",
                table: "account");

            migrationBuilder.DropColumn(
                name: "QuestionId1",
                table: "answer");

            migrationBuilder.RenameTable(
                name: "account",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_account_Username",
                table: "Account",
                newName: "IX_Accounts_Username");

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "department",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "behaviour",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Account",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Account",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Account",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiry",
                table: "Account",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_user_Account_AccountId",
                table: "user",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_department_school_SchoolId",
                table: "department");

            migrationBuilder.DropForeignKey(
                name: "FK_user_Account_AccountId",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_department_SchoolId",
                table: "department");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "department");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "behaviour");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiry",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "account");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_Username",
                table: "account",
                newName: "IX_account_Username");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId1",
                table: "answer",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "account",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "account",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_account",
                table: "account",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_answer_QuestionId1",
                table: "answer",
                column: "QuestionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_answer_question_QuestionId1",
                table: "answer",
                column: "QuestionId1",
                principalTable: "question",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_account_AccountId",
                table: "user",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
