using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoASPTrimestre3.Models;
using Rotativa;
using System.IO;
using System.Web.Routing;

namespace ProyectoASPTrimestre3.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        //public ActionResult Index()
        //{
        //    using (var db = new inventario2021Entities())
        //    {
        //        return View(db.producto.ToList());
        //    }
        //}

        //Muestra el nombre del proveedor en lugar del id
        public static string nombreProveedor (int idProveedor)
        {
            using (var db = new inventario2021Entities())
            {
                return db.proveedor.Find(idProveedor).nombre;
            }
        }

        //Lista de proveedores en el formulario
        public ActionResult listarProveedores()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.proveedor.ToList());
            }
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(producto producto)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.producto.Add(producto);
                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    return View(db.producto.Find(id));
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            using (var db = new inventario2021Entities())
            {
                producto productoEdit = db.producto.Where(a => a.id == id).FirstOrDefault();
                return View(productoEdit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(producto productoEdit)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var oldProducto = db.producto.Find(productoEdit.id);
                    oldProducto.nombre = productoEdit.nombre;
                    oldProducto.percio_unitario = productoEdit.percio_unitario;
                    oldProducto.descripcion = productoEdit.descripcion;
                    oldProducto.cantidad = productoEdit.cantidad;
                    oldProducto.id_proveedor = productoEdit.id_proveedor;
                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto producto = db.producto.Find(id);
                    db.producto.Remove(producto);
                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult EjReporte()
        {
            try
            {
                var db = new inventario2021Entities();
                var query = from tabProveedor in db.proveedor
                            join tabProducto in db.producto on tabProveedor.id equals tabProducto.id_proveedor
                            select new EjReporte
                            {
                                nombreProveedor = tabProveedor.nombre,
                                telefonoProveedor = tabProveedor.telefono,
                                direccionProveedor = tabProveedor.direccion,
                                nombreProducto = tabProducto.nombre,
                                precioProducto = tabProducto.percio_unitario
                            };
                return View(query);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(" ", "Error " + ex);
                return View();
            }
        }

        public ActionResult pdfReporte()
        {
            return new ActionAsPdf("EjReporte") { FileName = "reporte.pdf" };
        }

        public ActionResult PaginadorIndex(int pagina = 1)
        {
            try
            {
                var cantidadRegistros = 5;

                using (var db = new inventario2021Entities())
                {
                    var productos = db.producto.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistros).Take(cantidadRegistros).ToList();

                    var totalRegistros = db.producto.Count();
                    var modelo = new ModeloIndex();
                    modelo.Productos = productos;
                    modelo.ActualPage = pagina;
                    modelo.Total = totalRegistros;
                    modelo.RecordsPage = cantidadRegistros;
                    modelo.valueQueryString = new RouteValueDictionary();

                    return View(modelo);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

    }
}