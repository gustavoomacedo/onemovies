//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SESSAO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SESSAO()
        {
            this.POLTRONA = new HashSet<POLTRONA>();
        }
    
        public int SessaoId { get; set; }
        public int FilmeId { get; set; }
        public int SalaId { get; set; }
        public int CinemaId { get; set; }
        public string Horario { get; set; }
    
        public virtual CINEMA CINEMA { get; set; }
        public virtual FILME FILME { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POLTRONA> POLTRONA { get; set; }
        public virtual SALA SALA { get; set; }
    }
}
