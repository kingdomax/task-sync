using Microsoft.AspNetCore.Mvc;
using TaskSync.Models.Dto;
using TaskSync.Services.Interfaces;

namespace TaskSync.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("getTasks/{projectId}")]
        public async Task<IActionResult> GetTasks([FromRoute] int projectId)
        {
            var tasks = await _taskService.GetTasksAsync(projectId);

            return Ok(new TaskResponse() { Tasks = tasks });
        }
    }
}
