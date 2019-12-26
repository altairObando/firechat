using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Firechat.Models
{
    public class Participacion
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int ConversacionId { get; set; }
        public TipoParticipacion TipoParticipacion { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Conversacion Conversacion { get; set; }
    }
    public enum TipoParticipacion
    {
        Simple, Grupo
    }
}