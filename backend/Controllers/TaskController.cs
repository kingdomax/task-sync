using Microsoft.AspNetCore.Mvc;

using TaskSync.Controllers.TempDto;
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

        [HttpPost("addTask")]
        public async Task<IActionResult> AddTask([FromBody] AddTaskRequest request)
        {
            var taskDto = await _taskService.AddTaskAsync(request);
            return StatusCode(StatusCodes.Status201Created, taskDto);
        }

        [HttpPatch("updateStatus/{taskId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int taskId, [FromBody] UpdateTaskRequest request) // [FromRoute], [FromBody] are called ModelBining
        {
            var taskDto = await _taskService.UpdateTaskStatusAsync(taskId, request);

            return taskDto != null ? Ok(taskDto) : BadRequest("Task not found");
        }

        [HttpDelete("deleteTask/{taskId}")]
        public async Task<IActionResult> AddTask([FromRoute] int taskId)
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
