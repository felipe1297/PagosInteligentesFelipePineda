using System;
using System.Collections.Generic;

#nullable disable

namespace PruebaTecnica.Models
{
    public partial class Persona
    {
        public int IdPersona { get; set; }
        public string NombresPersona { get; set; }
        public string ApellidosPersona { get; set; }
        public string IdentificacionPersona { get; set; }
        public string CelularPersona { get; set; }
        public string DireccionPersona { get; set; }
        public string CiudadPersona { get; set; }
        public string CorreoPersona { get; set; }
    }
}
