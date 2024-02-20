var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(
    options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    }
);
app.MapGroup("methods")
    .MapMethodHelp()
    .MapHttpMethods();
app.MapGroup("status")
    .MapStatusHelp()
    .MapHttpStatuses()
    .WithOpenApi();
//TODO add callback where the user can request a call back to their own server

app.Run();

public static class RouteBuilderExtension
{
    private static readonly int[][] StatusCodesList =
    [
        [
            100, 101, 102, 103
        ],
        [
            200, 201, 202, 203, 204, 205, 206, 207, 208, 226
        ],
        [
            300, 301, 302, 303, 304, 305, 306, 307, 308
        ],
        [
            400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420,
            421, 422, 423, 424, 425, 426, 428, 429, 431, 451
        ],
        [
            500, 501, 502, 503, 504, 505, 506, 507, 508, 510, 511
        ]
    ];

    private static readonly string[] MethodsList =
    [
        "DELETE", "GET", "HEAD", "OPTIONS", "PATCH", "POST", "PUT", "TRACE"
    ];

    public static RouteGroupBuilder MapMethodHelp(this RouteGroupBuilder group)
    {
        group.MapGet("/help", () => Results.Redirect("https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods"))
            .WithTags("Help");
        return group;
    }

    public static RouteGroupBuilder MapHttpMethods(this RouteGroupBuilder group)
    {
        foreach (var method in MethodsList)
        {
            group.MapMethods("/", new[] { method }, () => method)
                .WithTags("Methods");
        }

        return group;
    }

    public static RouteGroupBuilder MapStatusHelp(this RouteGroupBuilder group)
    {
        group.MapGet("/help", () => Results.Redirect("https://developer.mozilla.org/en-US/docs/Web/HTTP/Status"))
            .WithTags("Help");
        return group;
    }

    public static RouteGroupBuilder MapHttpStatuses(this RouteGroupBuilder group)
    {
        foreach (var statusArray in StatusCodesList)
        {
            foreach (var status in statusArray)
            {
                foreach (var method in new[] { "GET" })
                {
                    group.MapMethods($"/{status}", new[] { method }, () => Results.StatusCode(status))
                        .WithTags($"Status: {status.ToString()[0] - 48}xx")
                        .Produces(status);
                }
            }
        }

        return group;
    }
}
