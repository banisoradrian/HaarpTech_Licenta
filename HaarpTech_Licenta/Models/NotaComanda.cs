using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarpTech_Licenta.Models
{
    [NotMapped]
    public class NotaComanda
    {
        public string ID_NOTA_COMANDA { get; set; }
        public string ID_CERERE{ get; set; }
        public string Destinatar { get; set; }
        public DateTime DataNota{ get; set; }
        public DateTime DataEmiterii { get; set; }
        public string Activitate { get; set; }
        public string Denumire { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "U.M. nu poate fi negativ.")]
        public int Um { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Cantitate nu poate fi negativă.")]
        public int Cantitate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Prețul fără TVA nu poate fi negativ.")]
        public decimal PretTotalFaraTva { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Prețul cu TVA nu poate fi negativ.")]
        public decimal PretTotalTva { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Termenul de livrare nu poate fi negativ.")]
        public int TermenLivrare { get; set; } // numar de zile

        public string Iban { get; set; }
        public string Banca { get; set; }
        public string NumarComanda { get; set; }
    }
}
