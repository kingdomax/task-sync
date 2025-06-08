using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskSync.Models;
using TaskSync.Models.Dto;
using TaskSync.Services.Interfaces;

namespace TaskSync.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("projects/{projectId}/tasks")]
        public async Task<IActionResult> GetTasks([FromRoute] int projectId)
        {
            var tasks = await _taskService.GetTasksAsync(projectId);

            return Ok(new KanbanBoardVm() { Tasks = tasks });
        }

        [Authorize]
        [HttpPost("projects/{projectId}/tasks")]
        public async Task<IActionResult> AddTask([FromRoute] int projectId, [FromBody] AddTaskRequest request)
        {
            var taskDto = await _taskService.AddTaskAsync(projectId, request);
            return StatusCode(StatusCodes.Status201Created, taskDto);
        }

        [Authorize]
        [HttpPatch("tasks/{taskId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int taskId, [FromBody] UpdateTaskRequest request) // [FromRoute], [FromBody] are called ModelBining
        {
            var taskDto = await _taskService.UpdateTaskStatusAsync(taskId, request);

            return taskDto != null ? Ok(taskDto) : BadRequest("Task not found");
        }

        [Authorize]
        [HttpDelete("tasks/{taskId}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int taskId)
        {
            var isSucess = await _taskService.DeleteTaskAsync(taskId);
            return isSucess ? NoContent() : NotFound("Task not found");
        }

        [HttpPost("testSomething/{param1}")]
        public IActionResult Test([FromRoute] int param1, [FromQuery] QueryStringDto queryStringDto)
        {
            return Ok("success");
        }
    }
}
