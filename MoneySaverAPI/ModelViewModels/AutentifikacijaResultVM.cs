using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneySaverAPI.ModelViewModels
{
    public class AutentifikacijaResultVM
    {
        public int KorisnikId { get; internal set; }
        public string KorisnickoIme { get; internal set; }
        public string LozinkaSalt { get; internal set; }
        public string Token { get; internal set; }
    }
}