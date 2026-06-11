using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConectaEleitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Posts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlanLimits_PlanId",
                table: "PlanLimits");

            migrationBuilder.CreateTable(
                name: "AssemblymanPostCategories",
                columns: table => new
                {
                    AssemblymanPostCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblymanPostCategories", x => x.AssemblymanPostCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "PoliticalProfile",
                columns: table => new
                {
                    PoliticalProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PhotoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BannerUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Office = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Party = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalProfile", x => x.PoliticalProfileId);
                });

            migrationBuilder.CreateTable(
                name: "AssemblymanPosts",
                columns: table => new
                {
                    AssemblymanPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsPinned = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    ViewsCount = table.Column<int>(type: "integer", nullable: false),
                    LikesCount = table.Column<int>(type: "integer", nullable: false),
                    CommentsCount = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblymanPosts", x => x.AssemblymanPostId);
                    table.ForeignKey(
                        name: "FK_AssemblymanPosts_AssemblymanPostCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AssemblymanPostCategories",
                        principalColumn: "AssemblymanPostCategoryId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AssemblymanPostComments",
                columns: table => new
                {
                    AssemblymanPostCommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblymanPostComments", x => x.AssemblymanPostCommentId);
                    table.ForeignKey(
                        name: "FK_AssemblymanPostComments_AssemblymanPostComments_ParentComme~",
                        column: x => x.ParentCommentId,
                        principalTable: "AssemblymanPostComments",
                        principalColumn: "AssemblymanPostCommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssemblymanPostComments_AssemblymanPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "AssemblymanPosts",
                        principalColumn: "AssemblymanPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssemblymanPostLikes",
                columns: table => new
                {
                    AssemblymanPostLikeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblymanPostLikes", x => x.AssemblymanPostLikeId);
                    table.ForeignKey(
                        name: "FK_AssemblymanPostLikes_AssemblymanPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "AssemblymanPosts",
                        principalColumn: "AssemblymanPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanLimits_PlanId_Type",
                table: "PlanLimits",
                columns: new[] { "PlanId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPostCategories_OwnerId",
                table: "AssemblymanPostCategories",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPostCategories_OwnerId_Name",
                table: "AssemblymanPostCategories",
                columns: new[] { "OwnerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPostComments_ParentCommentId",
                table: "AssemblymanPostComments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPostComments_PostId",
                table: "AssemblymanPostComments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPostComments_UserId",
                table: "AssemblymanPostComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPostLikes_PostId",
                table: "AssemblymanPostLikes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPostLikes_PostId_UserId",
                table: "AssemblymanPostLikes",
                columns: new[] { "PostId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPostLikes_UserId",
                table: "AssemblymanPostLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPosts_CategoryId",
                table: "AssemblymanPosts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPosts_CreatedAt",
                table: "AssemblymanPosts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPosts_IsPublished",
                table: "AssemblymanPosts",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPosts_OwnerId",
                table: "AssemblymanPosts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblymanPosts_UserId",
                table: "AssemblymanPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalProfile_City_State",
                table: "PoliticalProfile",
                columns: new[] { "City", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalProfile_IsActive",
                table: "PoliticalProfile",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalProfile_IsVerified",
                table: "PoliticalProfile",
                column: "IsVerified");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalProfile_OwnerId",
                table: "PoliticalProfile",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalProfile_OwnerId_UserId",
                table: "PoliticalProfile",
                columns: new[] { "OwnerId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalProfile_UserId",
                table: "PoliticalProfile",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssemblymanPostComments");

            migrationBuilder.DropTable(
                name: "AssemblymanPostLikes");

            migrationBuilder.DropTable(
                name: "PoliticalProfile");

            migrationBuilder.DropTable(
                name: "AssemblymanPosts");

            migrationBuilder.DropTable(
                name: "AssemblymanPostCategories");

            migrationBuilder.DropIndex(
                name: "IX_PlanLimits_PlanId_Type",
                table: "PlanLimits");

            migrationBuilder.CreateIndex(
                name: "IX_PlanLimits_PlanId",
                table: "PlanLimits",
                column: "PlanId");
        }
    }
}
