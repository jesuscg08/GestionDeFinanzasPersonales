using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GestionDeFinanzasPersonales.Models.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tipo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tipo()
        {
            this.Gestion = new HashSet<Gestion>();
        }

        [Display(Name = "ID Tipo")]
        public int IdTipo { get; set; }

        [Display(Name = "Tipo")]
        public string NombreTipo { get; set; }

        [Display(Name = "ID Categoria")]
        public Nullable<int> IdCategoria { get; set; }
    
        public virtual Categoria Categoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gestion> Gestion { get; set; }
    }
}
