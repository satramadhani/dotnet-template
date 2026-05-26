using Microsoft.AspNetCore.Mvc;
using SampleProject.API.Shared.Responses;
using SampleProject.Application.Configurations.Mediators;
using SampleProject.Application.Providers.Tests.Commands;
using SampleProject.Application.Providers.Tests.Dtos;

namespace SampleProject.API.Tests;

[ApiController]
[Route("tests")]

public class TestController(IMediator mediator) : ControllerBase
{
    [HttpGet("mediator")]
    [ProducesResponseType(typeof(ResponseBase<TestMediatorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TestMediatorAsync(CancellationToken cancellationToken)
    {
        var request = new TestMediatorCommand();
        var result = await mediator.SendAsync(request, cancellationToken);

        var response = AppResponse.Ok(result);
        return Ok(response);
    }
}
