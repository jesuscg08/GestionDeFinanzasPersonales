using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionDeFinanzasPersonales.Models.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Categoria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categoria()
        {
            this.Tipo = new HashSet<Tipo>();
        }
        [Display(Name = "ID Categoría")]
        public int IdCategoria { get; set; }

        [Display(Name = "Categoría")]
        public string NombreCategoria { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tipo> Tipo { get; set; }
    }
}
