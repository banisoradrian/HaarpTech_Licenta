using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarpTech_Licenta.Models
{
    
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Nume")]
        [Column("FIRST_NAME", TypeName = "VARCHAR(50)")]
        public string FirstName { get; set; } 

        [Required]
        [Column("LAST_NAME", TypeName = "VARCHAR(50)")]
        [Display(Name = "Prenume")]
        public string LastName { get; set; } 

        [Column("DATE_CREATED")]
        [Display(Name = "Data creării contului")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Țara")]
        [Column("COUNTRY", TypeName = "VARCHAR(30)")]
        public string? Country { get; set; }

        [Display(Name = "Nume Companie")]
        [Column("COMPANY_NAME", TypeName = "VARCHAR(100)")]
        public string? CompanyName { get; set; }

        [Display(Name = "Descriere Companie")]
        [Column("COMPANY_DESCRIPTION", TypeName = "VARCHAR(100)")]
        public string? CompanyDescription { get; set; }

        [Required(ErrorMessage = "Adresa de email este obligatorie.")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă.")]
        public override string Email { get; set; }

    }
}
