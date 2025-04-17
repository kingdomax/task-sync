using Microsoft.AspNetCore.Mvc;
using TaskSync.Services.Interfaces;

namespace TaskSync.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        
        public ProjectController(IProjectService _projectService)
        {
            _projectService = _projectService;
        }
    }
}
