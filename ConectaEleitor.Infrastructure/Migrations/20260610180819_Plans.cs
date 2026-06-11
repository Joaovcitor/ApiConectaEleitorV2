using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConectaEleitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Plans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MonthlyPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    YearlyPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "PlanUsages",
                columns: table => new
                {
                    PlanUsageId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    UsersCount = table.Column<int>(type: "integer", nullable: false),
                    VotersCount = table.Column<int>(type: "integer", nullable: false),
                    LeadersCount = table.Column<int>(type: "integer", nullable: false),
                    DemandsCount = table.Column<int>(type: "integer", nullable: false),
                    AppointmentsCount = table.Column<int>(type: "integer", nullable: false),
                    ReportsGeneratedCount = table.Column<int>(type: "integer", nullable: false),
                    ExportsGeneratedCount = table.Column<int>(type: "integer", nullable: false),
                    StorageUsedBytes = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanUsages", x => x.PlanUsageId);
                });

            migrationBuilder.CreateTable(
                name: "OwnerSubscriptions",
                columns: table => new
                {
                    OwnerSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    BillingCycle = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CurrentPeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CurrentPeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrialEndsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CanceledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SuspendedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerSubscriptions", x => x.OwnerSubscriptionId);
                    table.ForeignKey(
                        name: "FK_OwnerSubscriptions_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanFeatures",
                columns: table => new
                {
                    PlanFeatureId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanFeatures", x => x.PlanFeatureId);
                    table.ForeignKey(
                        name: "FK_PlanFeatures_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanLimits",
                columns: table => new
                {
                    PlanLimitId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: true),
                    IsUnlimited = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanLimits", x => x.PlanLimitId);
                    table.ForeignKey(
                        name: "FK_PlanLimits_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    PaymentTransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Provider = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    BillingCycle = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RefundedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExternalPaymentId = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    ExternalInvoiceId = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CheckoutUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PixQrCode = table.Column<string>(type: "text", nullable: true),
                    PixQrCodeBase64 = table.Column<string>(type: "text", nullable: true),
                    BoletoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FailureReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    MetadataJson = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.PaymentTransactionId);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_OwnerSubscriptions_OwnerSubscriptionId",
                        column: x => x.OwnerSubscriptionId,
                        principalTable: "OwnerSubscriptions",
                        principalColumn: "OwnerSubscriptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionHistory",
                columns: table => new
                {
                    SubscriptionHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    OldPlanId = table.Column<Guid>(type: "uuid", nullable: true),
                    NewPlanId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ChangedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionHistory", x => x.SubscriptionHistoryId);
                    table.ForeignKey(
                        name: "FK_SubscriptionHistory_OwnerSubscriptions_OwnerSubscriptionId",
                        column: x => x.OwnerSubscriptionId,
                        principalTable: "OwnerSubscriptions",
                        principalColumn: "OwnerSubscriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OwnerSubscriptions_OwnerId",
                table: "OwnerSubscriptions",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerSubscriptions_PlanId",
                table: "OwnerSubscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerSubscriptions_Status",
                table: "OwnerSubscriptions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_DueDate",
                table: "PaymentTransactions",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_ExternalInvoiceId",
                table: "PaymentTransactions",
                column: "ExternalInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_ExternalPaymentId",
                table: "PaymentTransactions",
                column: "ExternalPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_OwnerId",
                table: "PaymentTransactions",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_OwnerId_DueDate",
                table: "PaymentTransactions",
                columns: new[] { "OwnerId", "DueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_OwnerId_Status",
                table: "PaymentTransactions",
                columns: new[] { "OwnerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_OwnerSubscriptionId",
                table: "PaymentTransactions",
                column: "OwnerSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_PaidAt",
                table: "PaymentTransactions",
                column: "PaidAt");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_PaymentMethod",
                table: "PaymentTransactions",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_PlanId",
                table: "PaymentTransactions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_Provider",
                table: "PaymentTransactions",
                column: "Provider");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_Status",
                table: "PaymentTransactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFeatures_PlanId_Key",
                table: "PlanFeatures",
                columns: new[] { "PlanId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanLimits_PlanId",
                table: "PlanLimits",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Slug",
                table: "Plans",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanUsages_OwnerId",
                table: "PlanUsages",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanUsages_OwnerId_Year_Month",
                table: "PlanUsages",
                columns: new[] { "OwnerId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionHistory_CreatedAt",
                table: "SubscriptionHistory",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionHistory_OwnerSubscriptionId",
                table: "SubscriptionHistory",
                column: "OwnerSubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "PlanFeatures");

            migrationBuilder.DropTable(
                name: "PlanLimits");

            migrationBuilder.DropTable(
                name: "PlanUsages");

            migrationBuilder.DropTable(
                name: "SubscriptionHistory");

            migrationBuilder.DropTable(
                name: "OwnerSubscriptions");

            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}
