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
    
    public partial class Qdms_Cari
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Qdms_Cari()
        {
            this.Qdms_MusteriSikayet = new HashSet<Qdms_MusteriSikayet>();
        }
    
        public int ID { get; set; }
        public string CariNo { get; set; }
        public string CariAdi { get; set; }
        public string Adres { get; set; }
        public string VergiDairesi { get; set; }
        public string VergiNo { get; set; }
        public string Tel1Aciklama { get; set; }
        public string TUlkeKodu1 { get; set; }
        public string TAlanKodu1 { get; set; }
        public string Tel1 { get; set; }
        public string FUlkeKodu1 { get; set; }
        public string FAlanKodu1 { get; set; }
        public string Fax1 { get; set; }
        public string ElektronikPosta { get; set; }
        public string Ulke { get; set; }
        public string Bolge { get; set; }
        public string Yetkili { get; set; }
        public string Notlar { get; set; }
        public string Uyari { get; set; }
        public string CariTuru { get; set; }
        public string Semt { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string GSM { get; set; }
        public string WebAdres { get; set; }
        public string PostaKodu { get; set; }
        public string OdemeSekli { get; set; }
        public string TeslimSekli { get; set; }
        public string NakliyeSekli { get; set; }
        public string CalisilanDovizBirimi { get; set; }
        public Nullable<int> PlanlamaOnceligi { get; set; }
        public Nullable<bool> KullanimdaMi { get; set; }
        public string TicariUnvani { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Qdms_MusteriSikayet> Qdms_MusteriSikayet { get; set; }
    }
}
