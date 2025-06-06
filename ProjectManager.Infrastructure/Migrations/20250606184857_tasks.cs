using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Task_TaskId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Projects_ProjectId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskHistory_Task_TaskId",
                table: "TaskHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskHistory",
                table: "TaskHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "TaskHistory",
                newName: "TasksHistory");

            migrationBuilder.RenameTable(
                name: "Task",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_TaskHistory_TaskId",
                table: "TasksHistory",
                newName: "IX_TasksHistory_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_TaskId",
                table: "Comments",
                newName: "IX_Comments_TaskId");

            migrationBuilder.AddColumn<Guid>(
                name: "ResponsibleUserId",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TasksHistory",
                table: "TasksHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TasksHistory_Tasks_TaskId",
                table: "TasksHistory",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TasksHistory_Tasks_TaskId",
                table: "TasksHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TasksHistory",
                table: "TasksHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ResponsibleUserId",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "TasksHistory",
                newName: "TaskHistory");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Task");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_TasksHistory_TaskId",
                table: "TaskHistory",
                newName: "IX_TaskHistory_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "Task",
                newName: "IX_Task_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_TaskId",
                table: "Comment",
                newName: "IX_Comment_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskHistory",
                table: "TaskHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                table: "Task",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Task_TaskId",
                table: "Comment",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Projects_ProjectId",
                table: "Task",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHistory_Task_TaskId",
                table: "TaskHistory",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
