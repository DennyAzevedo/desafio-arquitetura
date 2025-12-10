using Microsoft.AspNetCore.Mvc;
using TransactionService.Api.Dtos;
using TransactionService.Application.Commands;
using TransactionService.Application.Handlers;
using TransactionService.Application.Services;
using TransactionService.Domain.Enums;

namespace TransactionService.Controllers;

[ApiController]
[Route("api/v1/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionApplicationService _applicationService;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionsController(TransactionApplicationService applicationService, ITransactionRepository transactionRepository)
    {
        _applicationService = applicationService;
        _transactionRepository = transactionRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.MerchantId))
            return BadRequest("MerchantId is required");

        if (dto.Amount <= 0)
            return BadRequest("Amount must be greater than zero");

        if (!Enum.TryParse<TransactionDirection>(dto.Direction, true, out var direction))
            return BadRequest("Invalid direction. Use 'Credit' or 'Debit'");

        var command = new CreateTransactionCommand(
            dto.MerchantId,
            dto.Amount,
            dto.Currency,
            direction,
            dto.OccurredAt
        );

        var transactionId = await _applicationService.CreateTransactionAsync(command, cancellationToken);
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, cancellationToken);

        var response = new TransactionResponseDto(
            transaction!.Id,
            transaction.MerchantId,
            transaction.Amount,
            transaction.Currency,
            transaction.Direction.ToString(),
            transaction.OccurredAt,
            transaction.CreatedAt
        );

        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransaction(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id, cancellationToken);
        
        if (transaction == null)
            return NotFound();

        var response = new TransactionResponseDto(
            transaction.Id,
            transaction.MerchantId,
            transaction.Amount,
            transaction.Currency,
            transaction.Direction.ToString(),
            transaction.OccurredAt,
            transaction.CreatedAt
        );

        return Ok(response);
    }
}
