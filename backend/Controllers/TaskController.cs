using Microsoft.AspNetCore.Mvc;

using TaskSync.Filters;
using TaskSync.Models.Dto;
using TaskSync.Services.Interfaces;

namespace TaskSync.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/task")]
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

            return Ok(new KanbanBoardVm() { Tasks = tasks });
        }

        [ValidateRequest]
        [HttpPatch("updateStatus/{taskId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int taskId, [FromBody] UpdateTaskRequest request)
        {
            var taskDto = await _taskService.UpdateTaskStatusAsync(taskId, request);

            return taskDto != null ? Ok(taskDto) : BadRequest("Task not found");
        }

        [ValidateRequest]
        [HttpPost("addTask")]
        public async Task<IActionResult> AddTask([FromBody] AddTaskRequest request)
        {
            var taskDto = await _taskService.AddTaskAsync(request);
            return Ok(taskDto);
        }
    }
}
