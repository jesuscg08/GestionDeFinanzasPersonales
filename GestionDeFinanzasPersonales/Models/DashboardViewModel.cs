using GestionDeFinanzasPersonales.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDeFinanzasPersonales.Models
{
    public class DashboardViewModel
    {
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal Balance { get; set; }

        public List<Gestion> Movimientos { get; set; }
    }
}