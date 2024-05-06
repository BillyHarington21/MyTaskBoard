using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Formats.Tar;
using System.Threading.Tasks;
using TaskBoard.Data;
using TaskBoard.Interfaces;
using TaskBoard.Models;
using TaskBoard.RoleRequired;
using TaskBoard.ViewModels;


namespace TaskBoard.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ISprintServices _sprintServices;
        private readonly ITaskService _taskService;
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _appEnvironment;
        

        public ProjectController(IProjectService projectService, ISprintServices sprintServices, AppDbContext appDbContext, ITaskService taskService, IWebHostEnvironment appEnvironment)
        {
            _projectService = projectService;
            _sprintServices = sprintServices;
            _appDbContext = appDbContext;
            _taskService = taskService;
            _appEnvironment = appEnvironment;
        }

        //методы для создания и обработки ПРОЕКТА

        public IActionResult AllProject()
        {
            var projects = _projectService.GetAllProjects();
            if (projects == null)
                return NotFound();
            return View(projects);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(ProjectVM project)
        {
            if (ModelState.IsValid)
            {
                _projectService.CreateProject(project);
                return RedirectToAction("AllProject", "Project");
            }
            return View(project);
        }

        [HttpPost]
        public IActionResult Delete(string name)
        {
            _projectService.DeleteProject(name);
            return RedirectToAction("AllProject", "Project");
        }

        // методы для создания и обработки СПРИНТА

        [HttpGet]
        public IActionResult CreateSp()
        {

            return View();
        }
        [HttpPost]
        public IActionResult CreateSp(SprintVM sprint)
        {
            if (ModelState.IsValid)
            {

                _sprintServices.CreateSprint(sprint);
                return RedirectToAction("AllProject", "Project");

            }
            return View(sprint);
        }
        [HttpGet]
        public IActionResult AllSprintsOfProjects(string ProjectName)
        {
            var userList = _appDbContext.AppUsers.Select(u => u.UserName).ToList();
            var projectName = ProjectName;            
            var sprints = _sprintServices.GetAllSprintsForProjects(ProjectName);
            if (sprints == null)
                return NotFound();
            ViewBag.UserList = userList;
            ViewBag.ProjectName = projectName;
            return View(sprints);
        }

        [HttpPost]
        public IActionResult AllSprintsOfProjects(string ProjectName, string userName, int sprintId)
        {
            var project = _appDbContext.Projects.Include(p => p.Sprints).FirstOrDefault(s => s.Name == ProjectName);
            var currentSprint = project.Sprints.FirstOrDefault(s => s.Id == sprintId);           
            _sprintServices.AddUserToSprint(userName, currentSprint);
            return RedirectToAction("AllSprintsOfProjects", new { ProjectName });
        }
        [HttpPost]
        public IActionResult DeleteSprints(int SprintId, string ProjectName)
        {
            var projectname = ProjectName;
            var project = _appDbContext.Projects.Include(p => p.Sprints).FirstOrDefault(s => s.Name == projectname);
            var sprints = project.Sprints.ToList();
            if (sprints == null)
            {
                return NotFound();
            }
            else
            {
                _sprintServices.DeleteSprint(SprintId, sprints);
            }
            return RedirectToAction("AllProject", "Project");
        }

        // методы для создания и обработки ЗАДАЧИ
        [HttpGet]
        public IActionResult CreatTask(int sprintId)
        {
            return View(new TaskWorkVM { SprintId = sprintId });
        }
        [HttpPost]
        public IActionResult CreatTask(TaskWorkVM Task, int sprintId)
        {

             if (ModelState.IsValid)
            {
                _taskService.CreateNewTask(Task, sprintId);
                return RedirectToAction("AllTasksOfSprint", new { sprintId });
            }
            return View(Task);
        }

        [HttpGet]
        public IActionResult AllTasksOfSprint(int sprintId)
        {
            
            var tasks = _taskService.GetAllTasksOfSprints(sprintId);
            ViewBag.SprintId = sprintId;
           
            return View(tasks);
        }
        [HttpPost]
        public IActionResult DeleteTasks(int TaskId, int SprintId)
        {
            var task = _appDbContext.Tasks.Find(TaskId);
            var q = task.SprintId;
            var SPRINT = _appDbContext.Sprints.FirstOrDefault(w => w.Id == q);
            var sprint = _appDbContext.Sprints.Include(p => p.Tasks).FirstOrDefault(s => s.Id == q);
            var tasks = sprint.Tasks.ToList();
            if (sprint == null)
            {
                return NotFound();
            }
            else
            {
                _taskService.DeleteTask(TaskId, tasks);
            }
            return RedirectToAction("AllTasksOfSprint", new { SprintId });
        }
        [HttpPost]
        public IActionResult UpdateTaskStatus(int taskId, TaskStatus status)
        {
            var task = _appDbContext.Tasks.FirstOrDefault(w => w.Id == taskId);
            if (task == null)
            {
                return NotFound();
            }
           
            _taskService.AddTaskStatus(task, status);
            var isCompleted = task.IsCompleted;
            return RedirectToAction("AllTasksOfSprint", new { sprintId = task.SprintId });


        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(int taskId, string description , List<IFormFile> file, int SprintId )
        {
            var task = _appDbContext.Tasks.Include(t => t.Files).FirstOrDefault(t => t.Id == taskId);
            var sprint = _appDbContext.Sprints.Include(s => s.Tasks).FirstOrDefault(w => w.Id == SprintId);
            if (task == null)
            {
                return NotFound();
            }
            task.Description = description;
            
            if (file != null && file.Count > 0)
            {
                foreach (var formFile in file)
                {
                    // Получите имя файла
                    var fileName = Path.GetFileName(formFile.FileName);

                    // Сгенерируйте уникальное имя файла
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

                    var directoryPath = Path.Combine(_appEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(directoryPath);
                    var filePath = Path.Combine(directoryPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    // Создайте экземпляр TaskFile и свяжите его с задачей
                    var taskFile = new TaskFile
                    {
                        FileName = uniqueFileName,
                        FilePath = filePath,
                        TaskId = taskId
                    };
                    _appDbContext.Files.Add(taskFile);
                }
                if (string.IsNullOrEmpty(task.Description))
                {
                    task.Description = description; // или другое значение по умолчанию
                }
                if ( sprint.Id == 0)
                {
                    sprint.Id = SprintId; // или другое значение по умолчанию
                }
                _appDbContext.SaveChanges();

            }

            return RedirectToAction("AllTasksOfSprint", new { id = taskId, sprintId = task.SprintId });
        }

        // Методы осуществляющие развилку по уровню доступа
        public IActionResult PagesForRole()
        {
            var userName = User.Identity.Name;

            // Извлекаем пользователя из базы данных по его имени
            var user = _appDbContext.AppUsers.FirstOrDefault(u => u.UserName == userName);

            // Проверяем, что пользователь найден
            if (user != null)
            {
                // Получаем роль пользователя и сохраняем ее в ViewBag
                ViewBag.UserRole = user.Role.ToString();
            }
            return View();
        }
        public IActionResult SprintsForLoginUsers()
        {
            var userName = User.Identity.Name;           
            var user = _appDbContext.AppUsers.FirstOrDefault(u => u.UserName == userName);
            var ID = user?.Id;
            var sprint = _appDbContext.Sprints.Where(u => u.UserId == ID)
                                              .Select(s => new SprintVM
                                               {
                                                   Name = s.Name,
                                                   Description = s.Description,
                                                   StartDate = s.StartDate,
                                                   EndDate = s.EndDate,
                                                   Id = s.Id

                                               })
                                              .ToList(); 
            return View(sprint);
        }
        public IActionResult AllTasksOfLoginUserSprint(int sprintId)
        {
            
            var tasks = _appDbContext.Tasks.Where(t => t.SprintId == sprintId)
                                           .Select(w => new TaskWorkVM
                                           {
                                               Name= w.Name,
                                               Description = w.Description,
                                               IsCompleted = (TaskWorkVM.TaskStatus)w.IsCompleted,
                                               Files = w.Files
                                           })
                                           .ToList();

           
            return View(tasks);
        }


    }
}
