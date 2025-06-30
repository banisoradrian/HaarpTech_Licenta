using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarpTech_Licenta.Models
{
    [NotMapped]
    public class ElementeFactura
    {
       public string ID_ELEMENT_FACTURA { get; set; }
       public string ID_FACTURA { get; set; }

        [Display(Name = "Nr. Crt")]
        public int NrCrt{ get; set; }

        [Display(Name = "DenumireProdus")]
        public string DenumireProdus { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Valoarea nu poate fi negativă.")]
        public int Um { get; set; }

        [Display(Name = "Cantitate")]
        [Range(0, int.MaxValue, ErrorMessage = "Cantitatea nu poate fi negativă.")]
        public int Cantitate { get; set; }

        [Display(Name = "Valoarea Fără TVA")]
        [Range(0, double.MaxValue, ErrorMessage = "Valoarea nu poate fi negativă.")]
        public decimal ValoareFaraTva { get; set; }

        [Display(Name = "Valoarea Cu TVA")]
        [Range(0, double.MaxValue, ErrorMessage = "Valoarea nu poate fi negativă.")]
        public decimal ValoareCuTva { get; set; }
    }
}
