using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarpTech_Licenta.Models
{
    [NotMapped]
    public class Contract
    {
        public string ID_CONTRACT { get; set; }
        public string ID_CERERE { get; set; }

        [Display(Name = "Număr Contract")]
        public string NumarContract { get; set; }

        [Display(Name = "Beneficiar")]
        public string NumeBeneficiar{ get; set; }

        [Display(Name = "Data Contractului")]
        public DateTime DataContract { get; set; }
        
    }
}
