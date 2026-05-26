using Microsoft.AspNetCore.Mvc;
using SampleProject.API.Shared.Responses;
using SampleProject.Application.Configurations.Mediators;
using SampleProject.Application.Providers.Tests.Commands;
using SampleProject.Application.Providers.Tests.Dtos;
using SampleProject.Application.Providers.Tests.Queries;

namespace SampleProject.API.Tests;

[ApiController]
[Route("tests")]
public class TestController(IMediator mediator) : ControllerBase
{
    [HttpGet("id-generator")]
    [ProducesResponseType(typeof(ResponseBase<long>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIdGeneration(CancellationToken cancellationToken)
    {
        var request = new TestIdGeneratorQuery();
        var result = await mediator.SendAsync(request, cancellationToken);

        var response = AppResponse.Ok(result);
        return Ok(response);
    }

    [HttpGet("id-generator/batch")]
    [ProducesResponseType(typeof(ResponseBase<long>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBatchIdGeneration(
        [FromQuery] int batchSize,
        CancellationToken cancellationToken)
    {
        var request = new TestBatchIdGeneratorQuery(batchSize);
        var result = await mediator.SendAsync(request, cancellationToken);

        var response = AppResponse.Ok(result);
        return Ok(response);
    }
    
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
