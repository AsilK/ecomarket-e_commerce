using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models;

public class LoginModel
{
    [Required(ErrorMessage = "kullanıcı adı belirtiniz")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "parola belirtiniz")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public string? ReturnUrl { get; set; }
}
