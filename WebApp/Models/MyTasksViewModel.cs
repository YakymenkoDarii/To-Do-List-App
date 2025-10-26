using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.DataClasses;

namespace WebApp.Models
{
    public class MyTasksViewModel
    {
        public IEnumerable<TodoTaskWebApiModel> Tasks { get; set; }

        public DataClasses.TaskStatus? CurrentFilter { get; set; }

        public string CurrentSort { get; set; }

        public string NameSortParam { get; set; }

        public string DateSortParam { get; set; }
    }
}
