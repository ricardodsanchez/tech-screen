namespace SseApi.Domain;

public sealed class Transfer
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
    public string From { get; init; } = string.Empty;
    public string To { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
