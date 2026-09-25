using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MantisNetMvc.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // 1. AspNetRoles
        migrationBuilder.CreateTable(
            name: "AspNetRoles",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        // 2. AspNetUsers (ApplicationUser)
        migrationBuilder.CreateTable(
            name: "AspNetUsers",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                RealName = table.Column<string>(type: "TEXT", maxLength: 191, nullable: false),
                GlobalAccessLevel = table.Column<int>(type: "INTEGER", nullable: false),
                Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                Protected = table.Column<bool>(type: "INTEGER", nullable: false),
                DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                LastVisit = table.Column<DateTime>(type: "TEXT", nullable: true),
                UserName = table.Column<string>(type: "TEXT", maxLength: 191, nullable: true),
                NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "TEXT", maxLength: 191, nullable: true),
                NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        // 3. MantisProjects
        migrationBuilder.CreateTable(
            name: "MantisProjects",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                ViewState = table.Column<int>(type: "INTEGER", nullable: false),
                Status = table.Column<int>(type: "INTEGER", nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                InheritCategories = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisProjects", x => x.Id);
            });

        // 4. MantisCustomFields
        migrationBuilder.CreateTable(
            name: "MantisCustomFields",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                Type = table.Column<int>(type: "INTEGER", nullable: false),
                PossibleValues = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                DefaultValue = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                ValidRegexp = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                AccessLevelRead = table.Column<int>(type: "INTEGER", nullable: false),
                AccessLevelWrite = table.Column<int>(type: "INTEGER", nullable: false),
                LengthMin = table.Column<int>(type: "INTEGER", nullable: false),
                LengthMax = table.Column<int>(type: "INTEGER", nullable: false),
                RequireReport = table.Column<bool>(type: "INTEGER", nullable: false),
                RequireUpdate = table.Column<bool>(type: "INTEGER", nullable: false),
                RequireResolved = table.Column<bool>(type: "INTEGER", nullable: false),
                RequireClosed = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisCustomFields", x => x.Id);
            });

        // 5. MantisTags
        migrationBuilder.CreateTable(
            name: "MantisTags",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisTags", x => x.Id);
                table.ForeignKey(
                    name: "FK_MantisTags_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        // 6. AspNetRoleClaims
        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 7. AspNetUserClaims
        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 8. AspNetUserLogins
        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                UserId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 9. AspNetUserRoles
        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            columns: table => new
            {
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                RoleId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 10. AspNetUserTokens
        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            columns: table => new
            {
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Value = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 11. MantisCategories
        migrationBuilder.CreateTable(
            name: "MantisCategories",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ProjectId = table.Column<int>(type: "INTEGER", nullable: true),
                Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                DefaultAssigneeId = table.Column<int>(type: "INTEGER", nullable: true),
                Status = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisCategories", x => x.Id);
                table.ForeignKey(
                    name: "FK_MantisCategories_AspNetUsers_DefaultAssigneeId",
                    column: x => x.DefaultAssigneeId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_MantisCategories_MantisProjects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "MantisProjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 12. MantisCustomFieldProjects
        migrationBuilder.CreateTable(
            name: "MantisCustomFieldProjects",
            columns: table => new
            {
                CustomFieldId = table.Column<int>(type: "INTEGER", nullable: false),
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                Sequence = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisCustomFieldProjects", x => new { x.CustomFieldId, x.ProjectId });
                table.ForeignKey(
                    name: "FK_MantisCustomFieldProjects_MantisCustomFields_CustomFieldId",
                    column: x => x.CustomFieldId,
                    principalTable: "MantisCustomFields",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MantisCustomFieldProjects_MantisProjects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "MantisProjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 13. MantisProjectHierarchies
        migrationBuilder.CreateTable(
            name: "MantisProjectHierarchies",
            columns: table => new
            {
                ParentProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                ChildProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                InheritChild = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisProjectHierarchies", x => new { x.ParentProjectId, x.ChildProjectId });
                table.ForeignKey(
                    name: "FK_MantisProjectHierarchies_MantisProjects_ChildProjectId",
                    column: x => x.ChildProjectId,
                    principalTable: "MantisProjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MantisProjectHierarchies_MantisProjects_ParentProjectId",
                    column: x => x.ParentProjectId,
                    principalTable: "MantisProjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        // 14. MantisProjectUsers
        migrationBuilder.CreateTable(
            name: "MantisProjectUsers",
            columns: table => new
            {
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                AccessLevel = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisProjectUsers", x => new { x.ProjectId, x.UserId });
                table.ForeignKey(
                    name: "FK_MantisProjectUsers_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MantisProjectUsers_MantisProjects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "MantisProjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 15. MantisProjectVersions
        migrationBuilder.CreateTable(
            name: "MantisProjectVersions",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                Version = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                Released = table.Column<bool>(type: "INTEGER", nullable: false),
                Obsolete = table.Column<bool>(type: "INTEGER", nullable: false),
                DateOrder = table.Column<DateTime>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisProjectVersions", x => x.Id);
                table.ForeignKey(
                    name: "FK_MantisProjectVersions_MantisProjects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "MantisProjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 16. MantisIssues
        migrationBuilder.CreateTable(
            name: "MantisIssues",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                ReporterId = table.Column<int>(type: "INTEGER", nullable: false),
                HandlerId = table.Column<int>(type: "INTEGER", nullable: true),
                DuplicateId = table.Column<int>(type: "INTEGER", nullable: true),
                Priority = table.Column<int>(type: "INTEGER", nullable: false),
                Severity = table.Column<int>(type: "INTEGER", nullable: false),
                Reproducibility = table.Column<int>(type: "INTEGER", nullable: false),
                Status = table.Column<int>(type: "INTEGER", nullable: false),
                Resolution = table.Column<int>(type: "INTEGER", nullable: false),
                ViewState = table.Column<int>(type: "INTEGER", nullable: false),
                CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                Summary = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Description = table.Column<string>(type: "TEXT", nullable: false),
                StepsToReproduce = table.Column<string>(type: "TEXT", nullable: false),
                AdditionalInformation = table.Column<string>(type: "TEXT", nullable: false),
                TargetVersion = table.Column<string>(type: "TEXT", nullable: false),
                FixedInVersion = table.Column<string>(type: "TEXT", nullable: false),
                Build = table.Column<string>(type: "TEXT", nullable: false),
                DateSubmitted = table.Column<DateTime>(type: "TEXT", nullable: false),
                LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisIssues", x => x.Id);
                table.ForeignKey(
                    name: "FK_MantisIssues_AspNetUsers_HandlerId",
                    column: x => x.HandlerId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_MantisIssues_AspNetUsers_ReporterId",
                    column: x => x.ReporterId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MantisIssues_MantisCategories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "MantisCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_MantisIssues_MantisProjects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "MantisProjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        // 17. MantisCustomFieldValues
        migrationBuilder.CreateTable(
            name: "MantisCustomFieldValues",
            columns: table => new
            {
                CustomFieldId = table.Column<int>(type: "INTEGER", nullable: false),
                IssueId = table.Column<int>(type: "INTEGER", nullable: false),
                Value = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisCustomFieldValues", x => new { x.CustomFieldId, x.IssueId });
                table.ForeignKey(
                    name: "FK_MantisCustomFieldValues_MantisCustomFields_CustomFieldId",
                    column: x => x.CustomFieldId,
                    principalTable: "MantisCustomFields",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MantisCustomFieldValues_MantisIssues_IssueId",
                    column: x => x.IssueId,
                    principalTable: "MantisIssues",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 18. MantisIssueAttachments
        migrationBuilder.CreateTable(
            name: "MantisIssueAttachments",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                IssueId = table.Column<int>(type: "INTEGER", nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                Title = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                DiskFileName = table.Column<string>(type: "TEXT", nullable: false),
                FileName = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                FileFolder = table.Column<string>(type: "TEXT", nullable: false),
                FileSize = table.Column<int>(type: "INTEGER", nullable: false),
                FileType = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Content = table.Column<byte[]>(type: "BLOB", nullable: true),
                DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisIssueAttachments", x => x.Id);
                table.ForeignKey(
                    name: "FK_MantisIssueAttachments_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MantisIssueAttachments_MantisIssues_IssueId",
                    column: x => x.IssueId,
                    principalTable: "MantisIssues",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 19. MantisIssueHistories
        migrationBuilder.CreateTable(
            name: "MantisIssueHistories",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                IssueId = table.Column<int>(type: "INTEGER", nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                FieldName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                OldValue = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                NewValue = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                Type = table.Column<int>(type: "INTEGER", nullable: false),
                DateModified = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisIssueHistories", x => x.Id);
                table.ForeignKey(
                    name: "FK_MantisIssueHistories_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MantisIssueHistories_MantisIssues_IssueId",
                    column: x => x.IssueId,
                    principalTable: "MantisIssues",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 20. MantisIssueMonitors
        migrationBuilder.CreateTable(
            name: "MantisIssueMonitors",
            columns: table => new
            {
                IssueId = table.Column<int>(type: "INTEGER", nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisIssueMonitors", x => new { x.IssueId, x.UserId });
                table.ForeignKey(
                    name: "FK_MantisIssueMonitors_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MantisIssueMonitors_MantisIssues_IssueId",
                    column: x => x.IssueId,
                    principalTable: "MantisIssues",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 21. MantisIssueNotes
        migrationBuilder.CreateTable(
            name: "MantisIssueNotes",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                IssueId = table.Column<int>(type: "INTEGER", nullable: false),
                ReporterId = table.Column<int>(type: "INTEGER", nullable: false),
                Note = table.Column<string>(type: "TEXT", nullable: false),
                ViewState = table.Column<int>(type: "INTEGER", nullable: false),
                TimeTrackingMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                DateSubmitted = table.Column<DateTime>(type: "TEXT", nullable: false),
                LastModified = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisIssueNotes", x => x.Id);
                table.ForeignKey(
                    name: "FK_MantisIssueNotes_AspNetUsers_ReporterId",
                    column: x => x.ReporterId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MantisIssueNotes_MantisIssues_IssueId",
                    column: x => x.IssueId,
                    principalTable: "MantisIssues",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // 22. MantisIssueRelationships
        migrationBuilder.CreateTable(
            name: "MantisIssueRelationships",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                SourceIssueId = table.Column<int>(type: "INTEGER", nullable: false),
                DestinationIssueId = table.Column<int>(type: "INTEGER", nullable: false),
                Type = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisIssueRelationships", x => x.Id);
                table.ForeignKey(
                    name: "FK_MantisIssueRelationships_MantisIssues_DestinationIssueId",
                    column: x => x.DestinationIssueId,
                    principalTable: "MantisIssues",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MantisIssueRelationships_MantisIssues_SourceIssueId",
                    column: x => x.SourceIssueId,
                    principalTable: "MantisIssues",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        // 23. MantisIssueTags
        migrationBuilder.CreateTable(
            name: "MantisIssueTags",
            columns: table => new
            {
                IssueId = table.Column<int>(type: "INTEGER", nullable: false),
                TagId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MantisIssueTags", x => new { x.IssueId, x.TagId });
                table.ForeignKey(
                    name: "FK_MantisIssueTags_MantisIssues_IssueId",
                    column: x => x.IssueId,
                    principalTable: "MantisIssues",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MantisIssueTags_MantisTags_TagId",
                    column: x => x.TagId,
                    principalTable: "MantisTags",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // Indexes
        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "AspNetRoles",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AspNetUsers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "AspNetUsers",
            column: "NormalizedUserName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MantisProjects_Name",
            table: "MantisProjects",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MantisCategories_ProjectId_Name",
            table: "MantisCategories",
            columns: new[] { "ProjectId", "Name" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MantisCategories_DefaultAssigneeId",
            table: "MantisCategories",
            column: "DefaultAssigneeId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisCustomFields_Name",
            table: "MantisCustomFields",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MantisCustomFieldProjects_ProjectId",
            table: "MantisCustomFieldProjects",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisProjectHierarchies_ChildProjectId",
            table: "MantisProjectHierarchies",
            column: "ChildProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisProjectUsers_UserId",
            table: "MantisProjectUsers",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisProjectVersions_ProjectId_Version",
            table: "MantisProjectVersions",
            columns: new[] { "ProjectId", "Version" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssues_ProjectId",
            table: "MantisIssues",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssues_ReporterId",
            table: "MantisIssues",
            column: "ReporterId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssues_HandlerId",
            table: "MantisIssues",
            column: "HandlerId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssues_CategoryId",
            table: "MantisIssues",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssueAttachments_IssueId",
            table: "MantisIssueAttachments",
            column: "IssueId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssueAttachments_UserId",
            table: "MantisIssueAttachments",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssueHistories_IssueId",
            table: "MantisIssueHistories",
            column: "IssueId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssueHistories_UserId",
            table: "MantisIssueHistories",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssueMonitors_UserId",
            table: "MantisIssueMonitors",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssueNotes_IssueId",
            table: "MantisIssueNotes",
            column: "IssueId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssueNotes_ReporterId",
            table: "MantisIssueNotes",
            column: "ReporterId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssueRelationships_DestinationIssueId",
            table: "MantisIssueRelationships",
            column: "DestinationIssueId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssueRelationships_SourceIssueId",
            table: "MantisIssueRelationships",
            column: "SourceIssueId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisIssueTags_TagId",
            table: "MantisIssueTags",
            column: "TagId");

        migrationBuilder.CreateIndex(
            name: "IX_MantisTags_Name",
            table: "MantisTags",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MantisTags_UserId",
            table: "MantisTags",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "AspNetRoleClaims");
        migrationBuilder.DropTable(name: "AspNetUserClaims");
        migrationBuilder.DropTable(name: "AspNetUserLogins");
        migrationBuilder.DropTable(name: "AspNetUserRoles");
        migrationBuilder.DropTable(name: "AspNetUserTokens");
        migrationBuilder.DropTable(name: "MantisCustomFieldValues");
        migrationBuilder.DropTable(name: "MantisCustomFieldProjects");
        migrationBuilder.DropTable(name: "MantisCustomFields");
        migrationBuilder.DropTable(name: "MantisIssueAttachments");
        migrationBuilder.DropTable(name: "MantisIssueHistories");
        migrationBuilder.DropTable(name: "MantisIssueMonitors");
        migrationBuilder.DropTable(name: "MantisIssueNotes");
        migrationBuilder.DropTable(name: "MantisIssueRelationships");
        migrationBuilder.DropTable(name: "MantisIssueTags");
        migrationBuilder.DropTable(name: "MantisTags");
        migrationBuilder.DropTable(name: "MantisIssues");
        migrationBuilder.DropTable(name: "MantisCategories");
        migrationBuilder.DropTable(name: "MantisProjectHierarchies");
        migrationBuilder.DropTable(name: "MantisProjectUsers");
        migrationBuilder.DropTable(name: "MantisProjectVersions");
        migrationBuilder.DropTable(name: "MantisProjects");
        migrationBuilder.DropTable(name: "AspNetRoles");
        migrationBuilder.DropTable(name: "AspNetUsers");
    }
}
