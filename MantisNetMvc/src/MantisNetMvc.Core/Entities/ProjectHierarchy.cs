using System.ComponentModel.DataAnnotations.Schema;

namespace MantisNetMvc.Core.Entities;

/// <summary>
/// Replaces mantis_project_hierarchy_table. Manages parent/child project nesting.
/// </summary>
public class ProjectHierarchy
{
    public int ParentProjectId { get; set; }
    public virtual Project ParentProject { get; set; } = null!;

    public int ChildProjectId { get; set; }
    public virtual Project ChildProject { get; set; } = null!;

    public bool InheritChild { get; set; } = false;

    [NotMapped]
    public bool InheritParent { get => InheritChild; set => InheritChild = value; }
}
