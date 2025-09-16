using SseApi.Domain;

namespace SseApi.Services;

public sealed class TransferService
{
    private readonly IAccountStore _accounts;
    private readonly ITransferStore _transfers;

    public TransferService(IAccountStore accounts, ITransferStore transfers)
    {
        _accounts = accounts;
        _transfers = transfers;
    }

    public async Task<(Transfer? transfer, string? error, int status)> CreateAsync(
        string from, string to, decimal amount, string? idempotencyKey, CancellationToken ct)
    {
        if (amount <= 0) return (null, "amount_must_be_positive", 400);
        if (from == to) return (null, "accounts_must_differ", 400);

        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            var existing = await _transfers.GetByIdempotencyKeyAsync(idempotencyKey!, ct);
            if (existing is not null)
            {
                return (existing, null, 201);
            }
        }

        var src = await _accounts.GetAsync(from, ct);
        var dst = await _accounts.GetAsync(to, ct);
        if (src is null || dst is null) return (null, "account_not_found", 404);
        if (src.Balance < amount) return (null, "insufficient_funds", 409);

        // Intentional concurrency issue: non‑atomic updates.
        src.Balance -= amount;
        dst.Balance += amount;
        await _accounts.UpdateAsync(src, ct);
        await _accounts.UpdateAsync(dst, ct);

        var transfer = new Transfer { From = from, To = to, Amount = amount };
        await _transfers.AddAsync(transfer, ct);
        if (!string.IsNullOrWhiteSpace(idempotencyKey))
            await _transfers.SaveIdempotencyKeyAsync(idempotencyKey!, transfer, ct);

        return (transfer, null, 201);
    }
}
