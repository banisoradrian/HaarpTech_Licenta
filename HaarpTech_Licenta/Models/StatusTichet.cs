using System.ComponentModel.DataAnnotations.Schema;

namespace HaarpTech_Licenta.Models
{
    [NotMapped]
    public class StatusTichet
    {
        public string ID_TICHET { get; set; }
        public string ID_CERINTA { get; set; }
        public string ID_ANGAJAT { get; set; }
        public DateTime DataCreare { get; set; }
        public DateTime? DataFinalizare { get; set; }
        public string Subiect { get; set; }
        public string Descriere { get; set; }
        public string Status_Tichet { get; set; }
        public string Prioritate { get; set; }
    }
}
