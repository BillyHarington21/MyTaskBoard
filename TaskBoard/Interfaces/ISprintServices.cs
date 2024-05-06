using TaskBoard.Models;
using TaskBoard.ViewModels;

namespace TaskBoard.Interfaces
{
    public interface ISprintServices
    {
       
        void AddUserToSprint(string userName, Sprint sprint);
        void CreateSprint(SprintVM sprint);                 
        void DeleteSprint(int sprintId, List<Sprint> sprints);
        IEnumerable<SprintVM> GetAllSprintsForProjects(string projectName);
        
    }
}
