using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("auth")]
    public IActionResult GetAuth()
    {
        return Unauthorized("You are not authorized");
    }

    [HttpGet("not-found")]
    public IActionResult GetNotFound()
    {
        return NotFound("The requested resource was not found");
    }

    [HttpGet("server-error")]
    public IActionResult GetServerError()
    {
        throw new Exception("Something went wrong on the server");
    }

    [HttpGet("bad-request")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("The request was invalid. A bad request");
    }
}
