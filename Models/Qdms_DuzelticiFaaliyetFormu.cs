//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PolyteksKaliteKontrolTakip.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Qdms_DuzelticiFaaliyetFormu
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> UygunsuzlukTarihi { get; set; }
        public string UygunsuzlukTespitSekli { get; set; }
        public string UygunsuzlukDepartmani { get; set; }
        public string UygunsuzlukDurumu { get; set; }
        public string UygunsuzlukBildiren { get; set; }
        public string UygunsuzlukNedeni { get; set; }
        public string UygunsuzlukInceleyen { get; set; }
        public string YapilacakFaaliyet { get; set; }
        public Nullable<System.DateTime> OnguruGerceklesmeTarihi { get; set; }
        public string AdSoyadCozum { get; set; }
        public Nullable<bool> DuzelticiFaaliyetTakibi1 { get; set; }
        public string SonucAciklama { get; set; }
        public Nullable<System.DateTime> SonucTakipTarihi { get; set; }
        public string AdSoyadSonuc { get; set; }
        public Nullable<bool> DuzelticiFaaliyetTakibi2 { get; set; }
        public string GGAciklama { get; set; }
        public Nullable<System.DateTime> GGTakipTarihi { get; set; }
        public string GGAdSoyad { get; set; }
    }
}
