using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneySaverAPI.ModelViewModels
{
    public class TrosakAddVM
    {
        public int AktivnostId { get; set; }
        public int Dan { get; set; }
        public int Mjesec { get; set; }
        public int Godina { get; set; }
        public double Iznos { get; set; }
        public string AktivnostGreska { get; set; }
        public string DatumGreska { get; set; }
        public string IznosGreska { get; set; }

    }
}