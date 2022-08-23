using Blog.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.LoadConfiguration();
builder.ConfigureAuthentication();
builder.ConfigureMvc();
builder.ConfigureServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCompression();
app.UseStaticFiles(); //every static file (image, css, js, html, etc will be placed in wwwroot directory)
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
