using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarpTech_Licenta.Models
{
    [NotMapped]
    public class CerintaProdus
    {
        
        public string ID_CERINTA { get; set; }
        public string ID_CERERE { get; set; }
        public string ID_ANGJAT { get; set; }

        [Display(Name = "Data Elaborare")]
        public DateTime DataElaborare { get; set; }

        [Display(Name = "Descriere Detaliată")]
        public string DescriereDetaliata { get; set; }
        public string Prioritate { get; set; }
        [Display(Name ="Status Cerintă")]
        public string StatusCerinta{get;set; }

        // Cereri oferte
        public CerereOferta CerereOferta { get; set; }

    }
}
