using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaarpTech_Licenta.Models
{
    [NotMapped]
    public class CerereOferta
    {
        [Key]
        public string ID_CERERE{ get; set; }

        public string ID_USER{ get; set; }

        [Display(Name = "Denumire Cerere")]
        public string DenumireCerere { get; set; }

        [Display(Name = "Data Cererii")]
        public DateTime? DataCereri { get; set; }

        [Display(Name = "Preț")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Pret { get; set; }

        [Display(Name = "Descriere")]
        public string Descriere { get; set; }

        [Display(Name = "Status Cerere")]
        public string StatusCerere { get; set; }

        [Display(Name = "Tip Aplicație")]
        public string TipAplicatie { get; set; }

        [Display(Name = "Servicii AI")]
        public string ServiciiAI { get; set; }

        [Display(Name = "Nivel de Securitate")]
        public string NivelDeSecuritate { get; set; }

        [Display(Name = "Bază de Date")]
        public string BazaDeDate { get; set; }

        [Display(Name = "Tip Logare")]
        public string TipLogare { get; set; }

        [Display(Name = "Servicii Cloud")]
        public string ServiciiCloud { get; set; }

        [Display(Name = "Tip Acces")]
        public string TipAccess { get; set; }

        [Display(Name = "Integrare cu Alte Sisteme")]
        public string IntegrareCuAlteSisteme { get; set; }

        [Display(Name = "Accesibilitate")]
        public string Accesibilitate { get; set; }

        [Display(Name = "Conturi Logate")]
        public string ConturiLogate { get; set; }

        [Display(Name = "Timp de Realizare")]
        public string TimpDeRealizare { get; set; }

        [Display(Name = "Tehnologie")]
        public string Tehnologie { get; set; }

        [Display(Name = "Specificații Tehnice")]
        public string SpecificatiiTehnice { get; set; }

        [Display(Name = "Buget Estimat")]
        public decimal? BugetEstimat { get; set; }

        [Display(Name = "Data Ofertei")]
        public DateTime? DataOfertei { get; set; }

        [Display(Name = "Volum de Date")]
        public int? VolumDeDate { get; set; }

        [Display(Name = "Status Ofertă")]
        public string StatusOferta { get; set; }

        [Display(Name = "Tip Ofertă")]
        public string TipOferta { get; set; }
    }
}
