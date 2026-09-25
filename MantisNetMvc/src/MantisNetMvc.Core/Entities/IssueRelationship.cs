using MantisNetMvc.Core.Enums;

namespace MantisNetMvc.Core.Entities;

/// <summary>
/// Replaces mantis_bug_relationship_table.
/// </summary>
public class IssueRelationship : BaseEntity
{
    public int SourceIssueId { get; set; }
    public virtual Issue SourceIssue { get; set; } = null!;

    public int DestinationIssueId { get; set; }
    public virtual Issue DestinationIssue { get; set; } = null!;

    public RelationshipType Type { get; set; } = RelationshipType.RelatedTo;
}
