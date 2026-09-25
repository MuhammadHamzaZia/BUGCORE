namespace MantisNetMvc.Core.Enums;

/// <summary>
/// Mirrors MantisBT issue resolution status codes.
/// </summary>
public enum ResolutionType
{
    Open = 10,
    Fixed = 20,
    Reopened = 30,
    UnableToReproduce = 40,
    NotFixable = 50,
    Duplicate = 60,
    NoChangeRequired = 70,
    Suspended = 80,
    WontFix = 90
}
