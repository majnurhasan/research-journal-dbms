using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class V010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaOfInterest",
                columns: table => new
                {
                    AreaOfInterestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ISCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaOfInterest", x => x.AreaOfInterestId);
                });

            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    AuthorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    MailingAddress = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    Affiliation = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "Editor",
                columns: table => new
                {
                    EditorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editor", x => x.EditorId);
                });

            migrationBuilder.CreateTable(
                name: "Issue",
                columns: table => new
                {
                    IssueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PublicationPeriod = table.Column<int>(nullable: false),
                    PublicationYear = table.Column<DateTime>(nullable: false),
                    VolumeNumber = table.Column<int>(nullable: false),
                    IssueNumber = table.Column<int>(nullable: false),
                    PrintDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issue", x => x.IssueId);
                });

            migrationBuilder.CreateTable(
                name: "Reviewer",
                columns: table => new
                {
                    ReviewerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    Affiliation = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviewer", x => x.ReviewerId);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_Author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Author",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Manuscript",
                columns: table => new
                {
                    ManuscriptId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EditorId = table.Column<int>(nullable: false),
                    IssueId = table.Column<int>(nullable: false),
                    ManuscriptTitle = table.Column<string>(nullable: true),
                    DateReceived = table.Column<DateTime>(nullable: false),
                    DateAccepted = table.Column<DateTime>(nullable: false),
                    ManuscriptStatus = table.Column<int>(nullable: false),
                    NumberOfPagesOccupied = table.Column<int>(nullable: false),
                    OrderInIssue = table.Column<int>(nullable: false),
                    BeginningPageNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manuscript", x => x.ManuscriptId);
                    table.ForeignKey(
                        name: "FK_Manuscript_Editor_EditorId",
                        column: x => x.EditorId,
                        principalTable: "Editor",
                        principalColumn: "EditorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manuscript_Issue_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issue",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreaOfInterestReviewer",
                columns: table => new
                {
                    AreaOfInterestReviewerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReviewerId = table.Column<int>(nullable: false),
                    AreaOfInterestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaOfInterestReviewer", x => x.AreaOfInterestReviewerId);
                    table.ForeignKey(
                        name: "FK_AreaOfInterestReviewer_AreaOfInterest_AreaOfInterestId",
                        column: x => x.AreaOfInterestId,
                        principalTable: "AreaOfInterest",
                        principalColumn: "AreaOfInterestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaOfInterestReviewer_Reviewer_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Reviewer",
                        principalColumn: "ReviewerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorManuscript",
                columns: table => new
                {
                    AuthorManuscriptId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<int>(nullable: false),
                    ManuscriptId = table.Column<int>(nullable: false),
                    AuthorOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorManuscript", x => x.AuthorManuscriptId);
                    table.ForeignKey(
                        name: "FK_AuthorManuscript_Author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Author",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorManuscript_Manuscript_ManuscriptId",
                        column: x => x.ManuscriptId,
                        principalTable: "Manuscript",
                        principalColumn: "ManuscriptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReviewerId = table.Column<int>(nullable: false),
                    ManuscriptId = table.Column<int>(nullable: false),
                    DateManuscriptReceived = table.Column<DateTime>(nullable: false),
                    AppropriatenessScore = table.Column<int>(nullable: false),
                    ClarityScore = table.Column<int>(nullable: false),
                    MethodologyScore = table.Column<int>(nullable: false),
                    ContributionScore = table.Column<int>(nullable: false),
                    RecommendationStatus = table.Column<bool>(nullable: false),
                    DateReviewed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Review_Manuscript_ManuscriptId",
                        column: x => x.ManuscriptId,
                        principalTable: "Manuscript",
                        principalColumn: "ManuscriptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_Reviewer_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Reviewer",
                        principalColumn: "ReviewerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaOfInterestReviewer_AreaOfInterestId",
                table: "AreaOfInterestReviewer",
                column: "AreaOfInterestId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaOfInterestReviewer_ReviewerId",
                table: "AreaOfInterestReviewer",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorManuscript_AuthorId",
                table: "AuthorManuscript",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorManuscript_ManuscriptId",
                table: "AuthorManuscript",
                column: "ManuscriptId");

            migrationBuilder.CreateIndex(
                name: "IX_Manuscript_EditorId",
                table: "Manuscript",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Manuscript_IssueId",
                table: "Manuscript",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_AuthorId",
                table: "Notification",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ManuscriptId",
                table: "Review",
                column: "ManuscriptId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ReviewerId",
                table: "Review",
                column: "ReviewerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaOfInterestReviewer");

            migrationBuilder.DropTable(
                name: "AuthorManuscript");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "AreaOfInterest");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "Manuscript");

            migrationBuilder.DropTable(
                name: "Reviewer");

            migrationBuilder.DropTable(
                name: "Editor");

            migrationBuilder.DropTable(
                name: "Issue");
        }
    }
}
