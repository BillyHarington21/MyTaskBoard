using TaskBoard.Models;
using TaskBoard.ViewModels;

namespace TaskBoard.Interfaces
{
    public interface ITaskService
    {
        void AddTaskStatus(TaskWork task, TaskStatus status);
        void CreateNewTask(TaskWorkVM Task, int sprintId);
        void DeleteTask(int TaskId, List<Models.TaskWork> tasks);
        IEnumerable<TaskWorkVM> GetAllTasksOfSprints(int SprintId);
        
    }
}
