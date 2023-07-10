using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4-8}$", ErrorMessage = "Password must be complex")]//en az bir sayısal karakter ((?=.*\\d)), en az bir küçük harf ((?=.*[a-z])), en az bir büyük harf ((?=.*[A-Z])) ve 4 ila 8 karakter uzunluğunda (.{4-8}$) olması gerektiğini belirtir. Eğer şifre bu düzene uymazsa, ErrorMessage özelliği aracılığıyla belirtilen hata mesajı kullanıcıya gösterilir.
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
