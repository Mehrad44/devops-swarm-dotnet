using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/info", async () =>
{
    var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "db";
    var db = Environment.GetEnvironmentVariable("DB_NAME") ?? "appdb";
    var user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
    var pass = Environment.GetEnvironmentVariable("DB_PASS") ?? "Secret123";
    var connStr = $"Host={host};Database={db};Username={user};Password={pass}";

    try
    {
        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        await using var createCmd = new NpgsqlCommand(
            "CREATE TABLE IF NOT EXISTS visits (id SERIAL PRIMARY KEY, container_id TEXT, visited_at TIMESTAMP);", conn);
        await createCmd.ExecuteNonQueryAsync();

        await using var insertCmd = new NpgsqlCommand(
            "INSERT INTO visits (container_id, visited_at) VALUES (@c, NOW());", conn);
        insertCmd.Parameters.AddWithValue("c", Environment.MachineName);
        await insertCmd.ExecuteNonQueryAsync();

        await using var countCmd = new NpgsqlCommand("SELECT COUNT(*) FROM visits;", conn);
        var totalVisits = Convert.ToInt64(await countCmd.ExecuteScalarAsync());

        return Results.Ok(new
        {
            Status = "Connected to PostgreSQL",
            BackendFramework = ".NET 8 Web API",
            HandledByContainer = Environment.MachineName,
            TotalDatabaseRecords = totalVisits,
            ServerTimeUtc = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new
        {
            Status = "Database initializing, try again in a moment",
            HandledByContainer = Environment.MachineName,
            Details = ex.Message
        });
    }
});

app.Run();
