using SseApi.Domain;

namespace SseApi.Services;

public interface ITransferStore
{
    Task<Transfer> AddAsync(Transfer transfer, CancellationToken ct = default);
    Task<IReadOnlyList<Transfer>> ListAsync(CancellationToken ct = default);
    Task<Transfer?> GetByIdempotencyKeyAsync(string key, CancellationToken ct = default);
    Task SaveIdempotencyKeyAsync(string key, Transfer transfer, CancellationToken ct = default);
}
