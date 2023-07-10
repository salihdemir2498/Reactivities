using API.Services;
using Domain;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Persistence;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config) 
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false; //password alfa numric olmayan karakterler mi olsun 'a false                                                   dedik,passwordde alfa numreic karakter kullanabiliriz.
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DataContext>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true, //Doğrulama anahtarını kontrol etmek için true olarak ayarlanır.
                            IssuerSigningKey = key, //Doğrulama anahtarı olarak önceki adımda oluşturulan key kullanılır.
                            ValidateIssuer = false, //Tokenin verenin (issuer) doğrulanıp doğrulanmayacağını belirlemek için false olarak ayarlanır. Bu örnekte, verenin doğrulanmasına gerek duyulmamaktadır.
                            ValidateAudience = false //Tokenin alıcının (audience) doğrulanıp doğrulanmayacağını belirlemek için false olarak ayarlanır. Bu örnekte, alıcının doğrulanmasına gerek duyulmamaktadır.
                        };
                    });



            services.AddScoped<TokenService>();

            return services;
        }
    }
}
