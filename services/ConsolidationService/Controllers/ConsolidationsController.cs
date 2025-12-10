using ConsolidationService.Application.Queries;
using ConsolidationService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsolidationService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ConsolidationsController : ControllerBase
{
    private readonly ConsolidationQueryService _queryService;

    public ConsolidationsController(ConsolidationQueryService queryService)
    {
        _queryService = queryService;
    }

    [HttpGet("daily")]
    public async Task<IActionResult> GetDailyBalance([FromQuery] Guid merchantId, [FromQuery] DateTime date)
    {
        if (merchantId == Guid.Empty)
            return BadRequest("MerchantId is required");

        var query = new GetDailyBalanceQuery(merchantId, date);
        var result = await _queryService.GetDailyBalanceAsync(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
