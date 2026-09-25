namespace MantisNetMvc.Core.Enums;

/// <summary>
/// Mirrors MantisBT issue relationships.
/// </summary>
public enum RelationshipType
{
    RelatedTo = 0,
    ParentOf = 1,
    ChildOf = 2,
    DuplicateOf = 3,
    HasDuplicate = 4
}
