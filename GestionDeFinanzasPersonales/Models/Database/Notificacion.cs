
namespace GestionDeFinanzasPersonales.Models.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notificacion
    {
        public int IdNotificacion { get; set; }
        public int IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public Nullable<bool> Leida { get; set; }
    
        public virtual Usuario Usuario { get; set; }
    }
}
