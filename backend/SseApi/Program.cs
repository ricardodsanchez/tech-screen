using SseApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAccountStore, InMemoryAccountStore>();
builder.Services.AddSingleton<ITransferStore, InMemoryTransferStore>();
builder.Services.AddSingleton<TransferService>();

// Intentional omission: builder.Services.AddControllers();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});


var app = builder.Build();

// Intentional omission: app.UseCors();
app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers(); // Will not map without AddControllers (intentional for discussion)
app.MapGet("/health", () => Results.Ok(new { ok = true }));

app.Run("http://localhost:5178");
