using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Firechat.Models
{
    public class Conversacion
    {
        public int Id { get; set; }
        [Display(Name="Creador")]
        public string ApplicationUserId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual List<Participacion> Participantes { get; set; }
        public virtual List<Mensaje> Mensajes { get; set; }
        public virtual List<ConversacionEliminada> ConversacionesBorradas { get; set; }
        public string UltimoMensaje { 
            get { 
                if(Mensajes.Count > 0)
                {
                    // Ordenar los mensajes de forma descendente y seleccionar el ultimo
                    return Mensajes.OrderByDescending(x => x.Id).FirstOrDefault().Contenido;
                }
                else
                {
                    return "Nueva Conversación";
                }
            }
        }

        public Conversacion()
        {
            Participantes = new List<Participacion>();
            Mensajes = new List<Mensaje>();
            ConversacionesBorradas = new List<ConversacionEliminada>();
        }
    }
    public class ConversacionEliminada
    {
        public int Id { get; set; }
        public int ConversacionId { get; set; }
        [Display(Name ="Eliminada para el usuario")]
        public string ApplicationUserId { get; set; }
        public DateTime Fecha { get; set; }
        public virtual Conversacion Conversacion { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}