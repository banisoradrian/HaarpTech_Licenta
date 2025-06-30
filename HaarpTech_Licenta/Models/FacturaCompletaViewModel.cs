namespace HaarpTech_Licenta.Models
{
    public class FacturaCompletaViewModel
    {
        public Factura Factura { get; set; }
        public List<ElementeFactura> ElementeFactura { get; set; }
        public ClientData Client { get; set; }

    }
}
