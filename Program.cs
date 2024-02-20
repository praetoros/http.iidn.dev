var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "The World!");
app.MapPost("/", () => "New World!");
app.MapPut("/", () => "Update (replace) World!");
app.MapDelete("/", () => "Delete World!");
app.MapPatch("/", () => "Update (merge) World!");

app.Run();
