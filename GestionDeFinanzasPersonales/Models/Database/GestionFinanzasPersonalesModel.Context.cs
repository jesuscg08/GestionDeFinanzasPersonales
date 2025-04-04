
namespace GestionDeFinanzasPersonales.Models.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GestionFinanzasPersonalesEntities2 : DbContext
    {
        public GestionFinanzasPersonalesEntities2()
            : base("name=GestionFinanzasPersonalesEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Gestion> Gestion { get; set; }
        public virtual DbSet<Tipo> Tipo { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<CategoriaPresupuesto> CategoriaPresupuesto { get; set; }
        public virtual DbSet<MetaFinanciera> MetaFinanciera { get; set; }
        public virtual DbSet<Notificacion> Notificacion { get; set; }
        public virtual DbSet<Presupuesto> Presupuesto { get; set; }
        public virtual DbSet<Recordatorio> Recordatorio { get; set; }
    }
}
