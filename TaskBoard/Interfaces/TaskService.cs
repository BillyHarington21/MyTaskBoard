using TaskBoard.Data;
using TaskBoard.Models;
using TaskBoard.ViewModels;

namespace TaskBoard.Interfaces
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _appDbContext;
        public TaskService(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }
        void ITaskService.CreateNewTask(TaskWorkVM task, int sprintId)
        {                     
                var newtask = new TaskWork
                {
                    Name = task.Name,
                    Description = task.Description,
                    SprintId  = task.SprintId,
                    IsCompleted = (TaskWork.TaskStatus)task.IsCompleted,
                    Files = task.Files,
                    Id = task.Id,
                    

                };
                _appDbContext.Tasks.Add(newtask);
                _appDbContext.SaveChanges(); 
          
        }
        IEnumerable<TaskWorkVM> ITaskService.GetAllTasksOfSprints(int SprintId)
        {
            return _appDbContext.Tasks
                             .Where(s => s.SprintId == SprintId)
                             .Select(s => new TaskWorkVM
                             { 
                                 Id = s.Id,
                                 Name = s.Name,
                                 Description = s.Description,
                                 SprintId = s.SprintId,
                                 IsCompleted = (TaskWorkVM.TaskStatus)s.IsCompleted,
                                 Files = s.Files
                             })
                             .ToList();
        }
        void ITaskService.DeleteTask(int TaskId, List<Models.TaskWork> tasks)
        {            
            var Task = tasks.FirstOrDefault(tasks => tasks.Id == TaskId);
            if (Task != null)
            {
                _appDbContext.Tasks.Remove(Task);
                _appDbContext.SaveChanges();
            }
        }
        void ITaskService.AddTaskStatus(TaskWork task, TaskStatus status)
        {
            task.IsCompleted = (TaskBoard.Models.TaskWork.TaskStatus)status;
            _appDbContext.SaveChanges();
        }
    }
}
