using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoASPTrimestre3.Models;
using System.IO;
using System.Web.Routing;

namespace ProyectoASPTrimestre3.Controllers
{
    public class Producto_CompraController : Controller
    {
        // GET: Producto_Compra
        //public ActionResult Index()
        //{
        //    using (var db = new inventario2021Entities())
        //    {
        //        return View(db.producto_compra.ToList());
        //    }
        //}

        public static string NombreProducto(int idProducto)
        {
            using (var db = new inventario2021Entities())
            {
                return db.producto.Find(idProducto).nombre;
            }
        }

        public ActionResult ListarProducto()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.producto.ToList());
            }
        }

        public static int TotalCompra(int idTotal)
        {
            using (var db = new inventario2021Entities())
            {
                return db.compra.Find(idTotal).id;
            }
        }

        public ActionResult ListarTotalCompra()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.compra.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(producto_compra producto_Compra)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.producto_compra.Add(producto_Compra);
                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError(" ", "Error " + ex);
                return View();
            }
        }

        public ActionResult Details(int idPcompra)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    return View(db.producto_compra.Find(idPcompra));
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError(" ", "Error " + ex);
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            using (var db = new inventario2021Entities())
            {
                producto_compra pCompraEdit = db.producto_compra.Where(a => a.id == id).FirstOrDefault();
                return View(pCompraEdit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(producto_compra pCompraEdit)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var oldpCompra = db.producto_compra.Find(pCompraEdit.id);
                    oldpCompra.id_compra = pCompraEdit.id_compra;
                    oldpCompra.id_producto = pCompraEdit.id_producto;
                    oldpCompra.cantidad = pCompraEdit.cantidad;

                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError(" ", "Error " + ex);
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto_compra producto_Compra = db.producto_compra.Find(id);
                    db.producto_compra.Remove(producto_Compra);
                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError(" ", "Error " + ex);
                return View();
            }
        }

        public ActionResult PaginadorIndex(int pagina = 1)
        {
            try
            {
                var cantidadRegistros = 5;

                using (var db = new inventario2021Entities())
                {
                    var producto_compras = db.producto_compra.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistros).Take(cantidadRegistros).ToList();

                    var totalRegistros = db.producto_compra.Count();
                    var modelo = new ModeloIndex();
                    modelo.Producto_Compras = producto_compras;
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