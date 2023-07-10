using Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            //Claims listesi oluşturulur ve kullanıcının adı (ClaimTypes.Name), kimlik belirleyici (ClaimTypes.NameIdentifier) ve e-posta adresi (ClaimTypes.Email) gibi bilgileri bu listede yer alır.

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])); //token 512 byte dan büyük olmalı
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Bir simetrik güvenlik anahtarı (SymmetricSecurityKey) oluşturulur ve bir algoritma (HmacSha512Signature) kullanarak anahtarı imzalamak için kullanılan bir SigningCredentials oluşturulur.

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            //Bir SecurityTokenDescriptor oluşturulur ve içerisine JWT'nin altında yer alan claims, geçerlilik süresi (Expires), imzalama bilgileri (SigningCredentials) gibi bilgiler eklenir.

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //Bir JwtSecurityTokenHandler oluşturulur ve CreateToken metodu ile SecurityTokenDescriptor kullanılarak bir JWT oluşturulur.

            return tokenHandler.WriteToken(token);


            //WriteToken bir JWT (JSON Web Token) nesnesini bir dizeye dönüştüren bir metoddur. Bu metod, oluşturulan JWT nesnesini dize biçiminde elde etmek için kullanılır.

            //WriteToken metodu genellikle JWT'yi dışarıya aktarmak veya iletmek için kullanılır. JWT, genellikle istemci tarafında kullanılmak üzere sunucudan alınır ve istemci tarafında kullanılabilmesi için bir dize olarak iletilir. Bu nedenle, WriteToken metodu, oluşturulan JWT'yi bir dize olarak döndürerek iletimini kolaylaştırır.
        }
    }
}
