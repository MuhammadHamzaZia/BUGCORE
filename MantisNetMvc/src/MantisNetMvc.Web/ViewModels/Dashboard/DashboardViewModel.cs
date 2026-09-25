using MantisNetMvc.Core.Entities;
using MantisNetMvc.Core.Interfaces;

namespace MantisNetMvc.Web.ViewModels.Dashboard;

public class DashboardViewModel
{
    public DashboardSummary Summary { get; set; } = new DashboardSummary();
    public Project? CurrentProject { get; set; }
    public IEnumerable<Project> AvailableProjects { get; set; } = new List<Project>();
    public MyViewBoxSettings Settings { get; set; } = new MyViewBoxSettings();
}
