using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace CINEMA_WEB3.Models;

public partial class User
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3 ile 50 karakter arasında olmalıdır.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")] 
    public string PasswordHash { get; set; } = null!; 

    [Display(Name = "Ad Soyad")] 
    public string? FullName { get; set; }

    [Required(ErrorMessage = "Rol boş bırakılamaz.")]
    public string Role { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}