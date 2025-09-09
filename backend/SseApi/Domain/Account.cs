namespace SseApi.Domain;

public sealed class Account
{
    public string Id { get; init; } = string.Empty;
    public decimal Balance { get; set; }
}
