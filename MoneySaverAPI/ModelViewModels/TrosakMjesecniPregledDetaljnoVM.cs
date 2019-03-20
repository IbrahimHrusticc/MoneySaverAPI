using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneySaverAPI.ModelViewModels
{
    public class TrosakMjesecniPregledDetaljnoVM
    {
        public int KorisnikId { get; set; }
        public double Trosak { get; set; }
        public int AktivnostId { get; set; }
        public string Aktivnost { get; set; }
        public double Ukupno { get; set; }
        public string MjesecGodina { get; set; }
        public int Mjesec { get; set; }
        public int Godina { get; set; }
        public List<TrosakMjesecniPregledDetaljnoVM> lista { get; set; }
    }
}