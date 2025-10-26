using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DataClasses;
using WebApp.Models;
using WebApp.Services.Interfaces;
using TaskStatus = WebApp.DataClasses.TaskStatus;

namespace WebApp.Controllers
{
    [Authorize]
    public class MyTasksController : Controller
    {
        private readonly ITodoTaskWebApiService service;
        private readonly UserManager<IdentityUser> userManager;

        public MyTasksController(ITodoTaskWebApiService service, UserManager<IdentityUser> userManager)
        {
            this.service = service;
            this.userManager = userManager;
        }

        // GET: /MyTasks/Index
        public async Task<IActionResult> Index(TaskStatus? filterStatus, string sortBy)
        {
            var userId = userManager.GetUserId(User);

            var dataTasks = await service.GetTasksForUserAsync(userId);

            bool defaultFilter = filterStatus == null;
            IEnumerable<TodoTask> filteredTasks;

            if (defaultFilter)
            {
                filteredTasks = dataTasks.Where(t =>
                    t.Status == TaskStatus.NotStarted || t.Status == TaskStatus.InProgress);
            }
            else
            {
                filteredTasks = dataTasks.Where(t => t.Status == filterStatus);
            }

            string nameSort = string.IsNullOrEmpty(sortBy) ? "name_desc" : "";
            string dateSort = sortBy == "date_asc" ? "date_desc" : "date_asc";

            var sortedTasks = filteredTasks;
            switch (sortBy)
            {
                case "name_desc":
                    sortedTasks = sortedTasks.OrderByDescending(t => t.Title);
                    break;
                case "date_asc":
                    sortedTasks = sortedTasks.OrderBy(t => t.DueTo);
                    break;
                case "date_desc":
                    sortedTasks = sortedTasks.OrderByDescending(t => t.DueTo);
                    break;
                default:
                    sortedTasks = sortedTasks.OrderBy(t => t.Title);
                    break;
            }

            var allUsers = await userManager.Users.ToListAsync();

            var taskViewModels = sortedTasks.Select(t => new TodoTaskWebApiModel
            {
                Id = t.Id,
                Title = t.Title,
                DueTo = t.DueTo,
                Status = t.Status,
                TodoListId = t.TodoListId,
                AssignedToUserName = allUsers.FirstOrDefault(u => u.Id == t.AssignedToId)?.UserName ?? "Unassigned",
            }).ToList();

            var viewModel = new MyTasksViewModel
            {
                Tasks = taskViewModels,
                CurrentFilter = filterStatus,
                CurrentSort = sortBy,
                NameSortParam = nameSort,
                DateSortParam = dateSort,
            };

            return View(viewModel);
        }

        // POST: /MyTasks/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int taskId, TaskStatus status)
        {
            var task = await this.service.GetByIdAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            var currentUserId = userManager.GetUserId(User);
            if (task.AssignedToId != currentUserId)
            {
                return Forbid();
            }

            task.Status = status;
            await service.UpdateAsync(task);

            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index");
        }
    }
}
