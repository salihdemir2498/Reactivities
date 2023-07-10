using API.Extensions;
using API.Middleware;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//istemcilerin yetkilendirilmemiş kullanıcılara erişimini engelleyen bir yetkilendirme politikası oluşturulur ve bu politika filtrelere eklenir.

    opt.Filters.Add(new AuthorizeFilter(policy)); // kontrolcülere gelen isteklerin yetkilendirme politikasına uygun olmasını sağlar. 

    //Bu şekilde, kontrolcülere gelen isteklerin, yetkilendirilmemiş kullanıcılardan gelen isteklerin engellenmesi için yetkilendirme politikasına tabi olması sağlanır.

});

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("CorsPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope(); //ASP.NET Core uygulamasındaki hizmetlerin kapsamını oluşturur. Bu, hizmet sağlayıcısına erişmek için bir kapsam (scope) oluşturur.
var services = scope.ServiceProvider; //kapsamdaki hizmet sağlayıcısına erişimi sağlar. Bu, bağımlılıkların çözümlenmesi için kullanılır.

try
{
    var context = services.GetRequiredService<DataContext>(); //DataContext tipindeki hizmeti alır. GetRequiredService<T>() metodu, verilen türdeki hizmeti alırken hizmetin bulunamaması durumunda bir hata fırlatır.
    var userManager = services.GetRequiredService<UserManager<AppUser>>(); 
    await context.Database.MigrateAsync(); //veritabanının migrasyon işlemini gerçekleştirir. Bu, mevcut veritabanının şema ve yapısını günceller veya oluşturur.
    await Seed.SeedData(context, userManager); // başlangıç verilerinin eklenmesini sağlayan bir yöntemi çağırır. Bu, uygulamanın başlatılmasıyla birlikte veritabanına bazı başlangıç verilerinin eklenmesini sağlar.
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>(); //ILogger<Program> türünde bir hizmet alır. Bu, hata günlüğü (error log) için bir kaydediciyi almak için kullanılır.
    logger.LogError(ex , "An error occured during migration"); //hata günlüğüne bir hata mesajı kaydeder. Hata günlüğüne kaydedilen hata mesajı, ex parametresi tarafından temsil edilen istisnayı içerir.
}

app.Run();
