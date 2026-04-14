using Skeleton.Repositories.Implementations;
using Skeleton.Repositories.Interfaces;
using Skeleton.Services.Implementations;
using Skeleton.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUsersRepository, UsersRepository>();


var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();