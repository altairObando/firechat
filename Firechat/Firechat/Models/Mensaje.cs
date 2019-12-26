using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Firechat.Models
{
    public class Mensaje
    {
        public int Id { get; set; }
        public int ConversacionId { get; set; }
        [Display(Name="Enviado por")]
        public string ApplicationUserId { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaEnvio { get; set; }
        public virtual Conversacion Conversacion { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual List<MensajeEliminado> MensajesEliminados { get; set; }
        public Mensaje()
        {
            this.MensajesEliminados = new List<MensajeEliminado>();
        }

    }
    public class MensajeEliminado
    {
        public int Id { get; set; }
        public int MensajeId { get; set; }
        [Display(Name ="Eliminado para el usuario")]
        public string ApplicationUserId  { get; set; }
        public DateTime FechaEliminacion { get; set; }
        public virtual Mensaje Mensaje { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
    public enum TiposDeMensajes
    {
        Texto, Imagen, Video, Audio
    }
}