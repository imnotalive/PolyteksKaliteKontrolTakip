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
    
    public partial class Qdms_MusteriSikayetAna
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Qdms_MusteriSikayetAna()
        {
            this.Qdms_MusteriSikayet = new HashSet<Qdms_MusteriSikayet>();
        }
    
        public int ID { get; set; }
        public Nullable<int> Firma { get; set; }
        public string MusteriTemsilcisi { get; set; }
        public Nullable<System.DateTime> GelisTarihi { get; set; }
        public Nullable<System.DateTime> SonucTarihi { get; set; }
        public string PartiNo { get; set; }
        public string Kalite { get; set; }
        public Nullable<System.DateTime> SevkTarihi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Qdms_MusteriSikayet> Qdms_MusteriSikayet { get; set; }
    }
}
