using Microsoft.EntityFrameworkCore;
using TaskBoard.Data;
using TaskBoard.Models;
using TaskBoard.ViewModels;

namespace TaskBoard.Interfaces
{
    public class ProjectServices : IProjectService
    {
       private readonly AppDbContext _appDbContext;

        public ProjectServices (AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        void IProjectService.CreateProject(ProjectVM project)
        {
            var newProject = new Project
            {
                Name = project.Name,
                Description = project.Description
            };
           _appDbContext.Projects.Add(newProject);
           _appDbContext.SaveChanges();
            
        }
        void IProjectService.DeleteProject(string Name)
        {
            var projectToDelete = _appDbContext.Projects.Find(Name);
            if (projectToDelete != null) 
            {
                _appDbContext.Projects.Remove(projectToDelete);
                _appDbContext.SaveChanges();
            }
        }
                
        IEnumerable<ProjectVM> IProjectService.GetAllProjects()
        {
            return _appDbContext.Projects.Select(p => new ProjectVM { Name = p.Name, Description = p.Description }).ToList();
            throw new NotImplementedException();
        }
    }
}
