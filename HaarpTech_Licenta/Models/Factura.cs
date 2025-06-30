using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaarpTech_Licenta.Models
{
        [NotMapped]
        public class Factura
        {
            [Key]
            [StringLength(450)]
            public string ID_FACTURA { get; set; }

            public string ID_CLIENT { get; set; }

            [Display(Name ="Seria Facturii")]
            public string SeriaFactura { get; set; }

            [Display(Name = "Numărul Facturii")]
            public string NumarFactura { get; set; }

            [Display(Name = "Data Emiterii")]
            public DateTime DataEmitere { get; set; }

            public string Moneda { get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "Valoarea nu poate fi negativă.")]
            public decimal? TotalFaraTva{ get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "Valoarea nu poate fi negativă.")]
            public decimal? TotalCuTva{ get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "Valoarea nu poate fi negativă.")]
            public decimal? TotalFactura { get; set; }
        }
   
}
