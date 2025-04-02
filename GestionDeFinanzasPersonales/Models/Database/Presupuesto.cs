
namespace GestionDeFinanzasPersonales.Models.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Presupuesto
    {
        public int IdPresupuesto { get; set; }
        public int IdUsuario { get; set; }
        public int IdCategoriaPresupuesto { get; set; }
        public decimal Monto { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
    
        public virtual CategoriaPresupuesto CategoriaPresupuesto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
