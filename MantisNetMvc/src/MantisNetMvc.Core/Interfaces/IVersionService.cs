using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Core.Interfaces;

public interface IVersionService
{
    Task<IEnumerable<ProjectVersion>> GetVersionsForProjectAsync(int projectId, bool includeObsolete = false);
    Task<ProjectVersion?> GetVersionByIdAsync(int id);
    Task<ProjectVersion> CreateVersionAsync(ProjectVersion version);
    Task<ProjectVersion> UpdateVersionAsync(ProjectVersion version);
    Task<bool> DeleteVersionAsync(int id);
    Task<bool> CopyVersionsAsync(int sourceProjectId, int targetProjectId);
}
