using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GestionDeFinanzasPersonales.Models.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Gestion
    {
        [Display(Name = "ID GESTIÓN")]
        public int IdGestion { get; set; }
        public decimal Monto { get; set; }

        [Display(Name = "ID Usuario")]
        public Nullable<int> IdUsuario { get; set; }

        [Display(Name = "ID Tipo")]
        public Nullable<int> IdTipo { get; set; }

        public virtual Tipo Tipo { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
