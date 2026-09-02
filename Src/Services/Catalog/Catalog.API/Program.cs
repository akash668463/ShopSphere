var builder = WebApplication.CreateBuilder(args);

//Add Services to the container

var app = builder.Build();

//configure the HTTP request pipline.

app.Run();
