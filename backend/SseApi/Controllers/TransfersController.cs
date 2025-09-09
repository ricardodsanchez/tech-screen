using Microsoft.AspNetCore.Mvc;
using SseApi.Services;

namespace SseApi.Controllers;

[ApiController]
[Route("transfers")]
public sealed class TransfersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromServices] TransferService svc, [FromBody] CreateTransfer body, CancellationToken ct)
    {
        var key = Request.Headers["Idempotency-Key"].FirstOrDefault();
        var (transfer, error, status) = await svc.CreateAsync(body.FromAccountId, body.ToAccountId, body.Amount, key, ct);
        return status switch
        {
            201 when transfer != null => Created($"/transfers/{transfer.Id}", transfer),
            400 => Problem(error, statusCode: 400),
            404 => Problem(error, statusCode: 404),
            409 => Problem(error, statusCode: 409),
            _ => Problem("unknown_error", statusCode: 500)
        };
    }

    [HttpGet]
    public async Task<IActionResult> List([FromServices] ITransferStore store, CancellationToken ct)
        => Ok(await store.ListAsync(ct));

    [HttpGet("/accounts")]
    public async Task<IActionResult> Accounts([FromServices] IAccountStore store, CancellationToken ct)
        => Ok(await store.ListAsync(ct));
}

public sealed record CreateTransfer(string FromAccountId, string ToAccountId, decimal Amount);
