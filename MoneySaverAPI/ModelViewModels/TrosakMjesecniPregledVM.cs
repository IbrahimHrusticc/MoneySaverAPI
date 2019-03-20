using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneySaverAPI.ModelViewModels
{
    public class TrosakMjesecniPregledVM
    {
        public int KorisnikId { get; set; }
        public double Trosak { get; set; }
        public string MjesecGodina { get; set; }
        public int TrosakId { get; set; }
        public List<TrosakMjesecniPregledVM> lista { get; set; }
    }
}