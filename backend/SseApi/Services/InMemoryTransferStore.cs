using SseApi.Domain;

namespace SseApi.Services;

public sealed class InMemoryTransferStore : ITransferStore
{
    // Intentional issue: no eviction and no lock; memory leak risk.
    private readonly List<Transfer> _transfers = new();
    private readonly Dictionary<string, Transfer> _idempotency = new();

    public Task<Transfer> AddAsync(Transfer transfer, CancellationToken ct = default)
    {
        _transfers.Add(transfer);
        return Task.FromResult(transfer);
    }

    public Task<IReadOnlyList<Transfer>> ListAsync(CancellationToken ct = default)
        => Task.FromResult((IReadOnlyList<Transfer>)_transfers.OrderByDescending(t => t.CreatedAt).ToList());

    public Task<Transfer?> GetByIdempotencyKeyAsync(string key, CancellationToken ct = default)
        => Task.FromResult(_idempotency.TryGetValue(key, out var t) ? t : null);

    public Task SaveIdempotencyKeyAsync(string key, Transfer transfer, CancellationToken ct = default)
    {
        _idempotency[key] = transfer; // overwrite without checking for conflict (intentional)
        return Task.CompletedTask;
    }
}
