using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Data;
using TaskBoard.Models;
using TaskBoard.ViewModels;

namespace TaskBoard.Interfaces
{
    public class SprintService : ISprintServices
    {
        
        private readonly AppDbContext _appDbContext;        
        public SprintService(AppDbContext appDbContext)
        {            
            _appDbContext = appDbContext;
        }
        void ISprintServices.CreateSprint(SprintVM sprint)
        {
            var project = _appDbContext.Projects.FirstOrDefault(p => p.Name == sprint.ProjectName);
            if (project != null)
            {
                var newSprint = new Sprint
                {
                   
                    Name = sprint.Name,
                    Description = sprint.Description,
                    StartDate = sprint.StartDate,
                    EndDate = sprint.EndDate,
                    ProjectName = sprint.ProjectName,
                    
                };

               _appDbContext.Sprints.Add(newSprint);
                _appDbContext.SaveChanges();
            }

        }
        IEnumerable<SprintVM> ISprintServices.GetAllSprintsForProjects(string projectName)
        {
            return _appDbContext.Sprints
                             .Where(s => s.ProjectName == projectName)
                             .Select(s => new SprintVM
                             {
                                 Name = s.Name,
                                 Description = s.Description,
                                 StartDate = s.StartDate,
                                 EndDate = s.EndDate,
                                 Id = s.Id,
                                 ProjectName = s.ProjectName
                                 
                             })
                             .ToList();
            throw new NotImplementedException();
        }
        void ISprintServices.DeleteSprint(int SprintId, List<Sprint> sprints)
        {
           
           var sprintToDelete = sprints.FirstOrDefault(s => s.Id == SprintId);
            if (sprintToDelete != null)
            {
                _appDbContext.Sprints.Remove(sprintToDelete);
                _appDbContext.SaveChanges();
            }
            
        }
        void ISprintServices.AddUserToSprint(string userName, Sprint sprint)
        {
            
            var appuser = _appDbContext.Users.FirstOrDefault(s => s.UserName == userName);            
            if (sprint != null)
            {
                sprint.UserId = appuser.Id;
                _appDbContext.SaveChanges();
            }
        }
    }
}
