using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneySaverAPI.ModelViewModels
{
    public class TrosakMjesecniPregledDetaljnoAktivnostiVM
    {
        public int KorisnikId { get; set; }
        public int TrosakId { get; set; }
        public double Iznos { get; set; }
        public string Datum { get; set; }
        public string Aktivnost { get; set; }
        public List<TrosakMjesecniPregledDetaljnoAktivnostiVM> lista { get; set; }

    }
}