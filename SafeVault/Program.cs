using SafeVault.Auth;
using SafeVault.Data;
using SafeVault.Middleware;
using SafeVault.Security;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Data Source=safevault.db";

DbInitializer.Initialize(connectionString);

var repo = new UserRepository(connectionString);
var auth = new AuthService(repo);

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapPost("/register", (RegisterRequest req) =>
{
    var username = InputSanitizer.Clean(req.Username);
    var email = InputSanitizer.Clean(req.Email);
    var password = req.Password ?? "";
    var role = InputSanitizer.Clean(req.Role);

    if (username == "" || email == "" || password == "" || role == "")
        return Results.BadRequest(new { error = "All fields are required." });

    if (!InputSanitizer.IsValidEmail(email))
        return Results.BadRequest(new { error = "Invalid email format." });

    if (InputSanitizer.LooksLikeScript(req.Username ?? "") || InputSanitizer.LooksLikeScript(req.Email ?? ""))
        return Results.BadRequest(new { error = "Potentially unsafe input detected." });

    var hash = auth.HashPassword(password);

    repo.CreateUser(username, email, hash, role);
    return Results.Ok(new { message = "User registered." });
});

app.MapPost("/login", (LoginRequest req) =>
{
    var username = InputSanitizer.Clean(req.Username);
    var password = req.Password ?? "";

    if (username == "" || password == "")
        return Results.BadRequest(new { error = "Username and password are required." });

    var result = auth.Login(username, password);

    // FIX #1: Unauthorized(...) can't take an object here, so return JSON with 401
    if (!result.ok)
        return Results.Json(new { error = result.message }, statusCode: 401);

    return Results.Ok(new { message = result.message, role = result.role });
});

app.MapGet("/admin/audit", (HttpContext ctx) =>
{
    var role = ctx.Request.Headers["X-Role"].ToString();

    // FIX #2: same issue here
    if (role != "Admin")
        return Results.Json(new { error = "Admin role required." }, statusCode: 401);

    return Results.Ok(new { message = "Audit logs (example)", logs = new[] { "Log1", "Log2" } });
});

app.Run();

record RegisterRequest(string Username, string Email, string Password, string Role);
record LoginRequest(string Username, string Password);
