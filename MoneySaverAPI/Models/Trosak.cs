//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MoneySaverAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Trosak
    {
        public int TrosakID { get; set; }
        public Nullable<System.DateTime> Datum { get; set; }
        public Nullable<decimal> Iznos { get; set; }
        public Nullable<int> AktivnostID { get; set; }
    
        public virtual Aktivnost Aktivnost { get; set; }
    }
}
