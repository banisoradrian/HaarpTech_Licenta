using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarpTech_Licenta.Models
{
    [NotMapped]
    public class RaportCerinta
    {
        public string ID_RAPORT { get; set; }
        public string Descriere { get; set; }

        [Required]
        [Display(Name = "Statusul Raportului")]
        public string StatusRaport { get; set; }

        [Required]
        public string Prioritate { get; set; }

        public string ID_SEDINTA { get; set; }

        [Display(Name = "Status Resurse")]
        public string StatusResurse { get; set; }

        [Display(Name = "Status Oferta")]
        public string StatusOferta { get; set; }

        public Sedinta Sedinte { get; set; }

    }
}
