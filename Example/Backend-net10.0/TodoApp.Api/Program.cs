var builder = WebApplication.CreateBuilder(args);

string DefaultCorsPolicy = "DefaultCorsPolicy";
builder.Services.AddCors(options =>
{
	// Development Cors Policy
	options.AddPolicy(name: DefaultCorsPolicy,
		builder =>
		{
			builder.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowAnyOrigin();
		});
});

// ArfBlocks Dependencies
builder.Services.AddArfBlocks(options =>
{
	options.ApplicationProjectNamespace = "TodoApp.Application";// nameof(TodoApp.Application);
	options.ConfigurationSection = builder.Configuration.GetSection("ProjectNameConfigurations");
	options.LogLevel = LogLevels.Information;
});

var app = builder.Build();

app.UseCors(DefaultCorsPolicy);

app.UseArfBlocksRequestHandlers(options =>
{
	// options.AuthorizationOptions.Audience = JwtService.Audience;
	// options.AuthorizationOptions.Secret = JwtService.Secret;
});

app.Run();
