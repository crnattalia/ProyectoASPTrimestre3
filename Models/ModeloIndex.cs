using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoASPTrimestre3.Models
{
    public class ModeloIndex : BaseModelo
    {
        public List<usuario> Usuarios { get; set; }

        public List<cliente> Clientes { get; set; }

        public List<compra> Compras { get; set; }

        public List<producto> Productos { get; set; }

        public List<producto_compra> Producto_Compras { get; set; }

        public List<proveedor> Proveedores { get; set; }

        public List<roles> Roles { get; set; }

        public List<usuariorol> Usuarioroles { get; set; }

    }
}