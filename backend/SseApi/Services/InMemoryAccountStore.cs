using SseApi.Domain;

namespace SseApi.Services;

public sealed class InMemoryAccountStore : IAccountStore
{
    // Intentional issue: not thread‑safe for concurrent updates.
    private readonly Dictionary<string, Account> _db = new()
    {
        ["A1"] = new Account { Id = "A1", Balance = 500.00m },
        ["A2"] = new Account { Id = "A2", Balance = 100.00m }
    };

    public Task<Account?> GetAsync(string id, CancellationToken ct = default)
        => Task.FromResult(_db.TryGetValue(id, out var acct) ? acct : null);

    public Task UpdateAsync(Account account, CancellationToken ct = default)
    {
        _db[account.Id] = account; // no locking
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Account>> ListAsync(CancellationToken ct = default)
        => Task.FromResult((IReadOnlyList<Account>)_db.Values.ToList());
}
