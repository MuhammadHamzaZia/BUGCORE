using MantisNetMvc.Core.Entities;
using MantisNetMvc.Core.Enums;

namespace MantisNetMvc.Core.Interfaces;

public interface IUserContextService
{
    int? GetCurrentUserId();
    Task<ApplicationUser?> GetCurrentUserAsync();
    int? GetActiveProjectId();
    void SetActiveProjectId(int? projectId);
    Task<AccessLevel> GetActiveProjectAccessLevelAsync();
}
