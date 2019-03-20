using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneySaverAPI.ModelViewModels
{
    public class AktivnostiResultVM
    {
        public List<Row> rows { get; set; }
        public string imaPodataka { get; set; }
        public class Row
        {
            public int AktivnostId { get;  set; }
            public string Naziv { get;  set; }

        }
    }
}