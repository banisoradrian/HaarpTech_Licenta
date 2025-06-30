namespace HaarpTech_Licenta.Models
{
    public class AssignTichetViewModel
    {
        public string ID_TICHET { get; set; }
        public string ID_ANGAJAT { get; set; }
        public string Subiect { get; set; }
        public IEnumerable<Angajat> Angajati { get; set; }   // pentru dropdown
    }

}
