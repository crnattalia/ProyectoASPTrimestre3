using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoASPTrimestre3.Models
{
    public class PrimerReporte
    {
        public String nombreUsuario { get; set; }
        public String apellidoUsuario { get; set; }
        public int totalCompra { get; set; }
        public DateTime fecha { get; set; }
        public String nombreCliente { get; set; }
        public String email { get; set; }
    }
}