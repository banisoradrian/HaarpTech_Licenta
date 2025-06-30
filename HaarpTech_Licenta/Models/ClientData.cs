using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarpTech_Licenta.Models
{
    [NotMapped]
    public class ClientData
    {
        public string ID_CLIENT { get; set; }
        public string ID_CONTRACT { get; set; }

        [Display(Name ="Denumire Fiscală")]
        public string DenumireFiscala { get; set; }

        public string Cui { get; set; }
        public string NrRegCom{ get; set; }
        public string Seduiu{ get; set; }

        public string Judet     {get;set;}
        public string Tara      {get;set;}

        [Display(Name = "Cod IBAN")]
        public string ContIban {get;set;}
        public string Banca     {get;set;}
        public string Telefon   {get;set;}
        public string Email { get; set; }
    }
}
