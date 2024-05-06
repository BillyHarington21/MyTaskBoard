using TaskBoard.ViewModels;

namespace TaskBoard.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectVM> GetAllProjects();
        void CreateProject(ProjectVM project);
        void DeleteProject(string Name);      
              
    }
}
 