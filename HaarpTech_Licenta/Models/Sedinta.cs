using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarpTech_Licenta.Models
{
    [NotMapped]
    public class Sedinta
    {
        public string ID_SEDINTA { get; set; }

        [Required]
        public DateTime DataSedinta { get; set; }
        
        [Required]
        public string Subiect { get; set; }
        
        [Required]
        public string Descriere { get; set; }
        
        public string StatusSedinta { get; set; }
        public string Locatie { get; set; }

        [Column("TIP_SEDINTE")]
        public string TipSedinta { get; set; }

        public string ID_USER { get; set; } // Foreign key catre Identity.User
        public string ID_CERERE { get; set; }
        public ApplicationUser User { get; set; } // Proprietate de navigare (optional)
    }
}
