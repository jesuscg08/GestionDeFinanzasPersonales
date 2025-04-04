using GestionDeFinanzasPersonales.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDeFinanzasPersonales.Models.Services
{
    public class NotificacionService
    {
        private readonly GestionFinanzasPersonalesEntities2 _db;
        public NotificacionService(GestionFinanzasPersonalesEntities2 dbContext) 
        {
            _db =dbContext;
        }

        //CREAR LA NOTIFICACIÓN
        public void CrearNotificacionMeta(int idUsuario, string titulo, string mensaje, string tipo)
        {

            var notificacion = new Notificacion
            {
                IdUsuario = idUsuario,
                Titulo = titulo,
                Mensaje = mensaje,
                Tipo = tipo,
                FechaCreacion = DateTime.Now,
                Leida = false
            };

            _db.Notificacion.Add(notificacion);
            _db.SaveChanges();
        }
    }
}