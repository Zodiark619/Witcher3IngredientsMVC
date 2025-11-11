using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Witcher3IngredientsMVC.Migrations
{
    /// <inheritdoc />
    public partial class rewre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemLinks_Items_SourceItemId",
                table: "ItemLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemLinks",
                table: "ItemLinks");

            migrationBuilder.RenameColumn(
                name: "SourceItemId",
                table: "ItemLinks",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ItemLinks",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "ItemLinks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemLinks",
                table: "ItemLinks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ItemLinks_ItemId",
                table: "ItemLinks",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemLinks_Items_ItemId",
                table: "ItemLinks",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemLinks_Items_ItemId",
                table: "ItemLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemLinks",
                table: "ItemLinks");

            migrationBuilder.DropIndex(
                name: "IX_ItemLinks_ItemId",
                table: "ItemLinks");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "ItemLinks");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ItemLinks",
                newName: "SourceItemId");

            migrationBuilder.AlterColumn<int>(
                name: "SourceItemId",
                table: "ItemLinks",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemLinks",
                table: "ItemLinks",
                columns: new[] { "SourceItemId", "ResultItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ItemLinks_Items_SourceItemId",
                table: "ItemLinks",
                column: "SourceItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
