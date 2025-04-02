
namespace GestionDeFinanzasPersonales.Models.Database
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    
    public partial class MetaFinanciera
    {
        public int IdMeta { get; set; }
        public int IdUsuario { get; set; }

        [Display(Name = "Nombre de la meta")]
        public string Nombre { get; set; }

        [Display(Name = "Tipo de la meta")]
        public string TipoMeta { get; set; }

        [Display(Name = "Monto objetivo")]
        public decimal MontoObjetivo { get; set; }

        [Display(Name = "Monto acumulado")]
        public Nullable<decimal> MontoAcumulado { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de inicio")]
        public System.DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha objetivo")]
        public System.DateTime FechaObjetivo { get; set; }
    
        public virtual Usuario Usuario { get; set; }
    }
}
