using System.ComponentModel.DataAnnotations;

namespace HaarpTech_Licenta.Models
{
    public class Angajat
    {
        public string ID_ANGAJAT { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Email{ get; set; }
        public string Telefon { get; set; }

        [Display(Name = "Dată Angajare")]
        public DateTime DataAngajare{ get; set; }

        public string Cnp { get; set; }

        [Display(Name = "Serie & NR")]
        public string SerieNr { get; set; }

        public string Localitate { get; set; }
        public string Tara { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salariul nu poate fi negativ.")]
        public decimal Salariu { get; set; }

        public string Pozitie { get; set; }
        public string Departament { get; set;}

        public string NumeComplet => $"{Nume} {Prenume}";

        public string AssignInfo => $"{Nume} {Prenume} - Departament: {Departament} , Pozitie: {Pozitie}";

    }
}
