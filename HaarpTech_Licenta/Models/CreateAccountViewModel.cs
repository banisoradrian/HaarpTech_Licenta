using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarpTech_Licenta.Models
{
    [NotMapped]
    public class CreateAccountViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "Parola trebuie să conțină cel puțin o literă mare, o literă mică, o cifră și un caracter special.")]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "Parola trebuie să conțină cel puțin o literă mare, o literă mică, o cifră și un caracter special.")]
        [Compare("Password", ErrorMessage = "Parola și confirmarea parolei nu se potrivesc.")]
        [Display(Name = "Confirma Parola")]
        public string ConfirmPassword { get; set; }



        [Required]
        [Display(Name = "Prenume")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nume")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Țara")]
        public string Country { get; set; }

        [Display(Name = "Nume companie")]
        public string CompanyName { get; set; }

        [Display(Name = "Descriere Companie")]
        public string CompanyDescription { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; } = "12314";
    }
}
