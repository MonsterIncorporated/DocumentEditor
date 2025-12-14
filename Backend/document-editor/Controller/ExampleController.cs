using Microsoft.AspNetCore.Mvc;

namespace document_editor.Controller;

[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return await Task.FromResult(new OkObjectResult("Hello World!"));
    }
}