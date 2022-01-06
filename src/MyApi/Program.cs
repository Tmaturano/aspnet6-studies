var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return Results.Ok("Hello World");
});

app.MapGet("/name/{nome}", (string nome) =>
{
    return Results.Ok($"Hello {nome}");
});

app.MapPost("/", (User user) =>
{
    if (user.Email is null)
        user.Email = "test@test.com";

    return Results.Ok(user);
});

app.Run();

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}