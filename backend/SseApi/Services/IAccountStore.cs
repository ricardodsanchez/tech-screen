using SseApi.Domain;

namespace SseApi.Services;

public interface IAccountStore
{
    Task<Account?> GetAsync(string id, CancellationToken ct = default);
    Task UpdateAsync(Account account, CancellationToken ct = default);
    Task<IReadOnlyList<Account>> ListAsync(CancellationToken ct = default);
}
