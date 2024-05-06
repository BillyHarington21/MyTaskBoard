namespace TaskBoard.Models
{
    public class TaskWork
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public TaskStatus IsCompleted { get; set; }
        public int SprintId { get; set; }
        public Sprint? Sprint { get; set; }
        public enum TaskStatus
        {
            NotStarted,
            InProgress,
            Completed
        }
        public List<TaskFile>? Files { get; set; }
    }
}
