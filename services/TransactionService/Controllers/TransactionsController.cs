using Microsoft.AspNetCore.Mvc;
using TransactionService.Api.Dtos;
using TransactionService.Application.Commands;
using TransactionService.Application.Handlers;
using TransactionService.Domain.Enums;

namespace TransactionService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionApplicationService _applicationService;

    public TransactionsController(TransactionApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        if (!Enum.TryParse<TransactionDirection>(dto.Direction, true, out var direction))
            return BadRequest("Invalid direction. Use 'Credit' or 'Debit'");

        var command = new CreateTransactionCommand(
            dto.MerchantId,
            dto.Amount,
            dto.Currency,
            direction,
            dto.OccurredAt
        );

        var transaction = await _applicationService.CreateTransactionAsync(command);

        var response = new TransactionResponseDto(
            transaction.Id,
            transaction.MerchantId,
            transaction.Amount,
            transaction.Currency,
            transaction.Direction.ToString(),
            transaction.OccurredAt
        );

        return CreatedAtAction(nameof(CreateTransaction), new { id = transaction.Id }, response);
    }
}
