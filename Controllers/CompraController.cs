using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoASPTrimestre3.Models;
using Rotativa;

namespace ProyectoASPTrimestre3.Controllers
{
    public class CompraController : Controller
    {
        // GET: Compra
        public ActionResult Index()
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.compra.ToList());
            }
        }

        //Mostrar nombre del cliente
        public static string nombreCliente(int idCliente)
        {
            using (var db = new inventario2021Entities())
            {
                return db.cliente.Find(idCliente).nombre;
            }
        }

        //Lista de clientes
        public ActionResult ListarCliente()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.cliente.ToList());
            }
        }

        //Mostrar nombre del usuario
        public static string nombreUsuario(int idUsuario)
        {
            using (var db = new inventario2021Entities())
            {
                return db.usuario.Find(idUsuario).nombre;
            }
        }

        //Lista de usuarios - Vista parcial
        public ActionResult ListarUsuario()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.usuario.ToList());
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(compra compra)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.compra.Add(compra);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(" ", "error " + ex);
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    return View(db.compra.Find(id));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(" ", "error " + ex);
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            using (var db = new inventario2021Entities())
            {
                compra compraEdit = db.compra.Where(a => a.id == id).FirstOrDefault();
                return View(compraEdit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(compra compraEdit)
        {
            try 
            {
                using (var db = new inventario2021Entities())
                {
                    var oldCompra = db.compra.Find(compraEdit.id);
                    oldCompra.fecha = compraEdit.fecha;
                    oldCompra.total = compraEdit.total;
                    oldCompra.id_usuario = compraEdit.id_usuario;
                    oldCompra.id_cliente = compraEdit.id_cliente;
                    db.SaveChanges();
                    return RedirectToAction("Index");
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
                    compra compra = db.compra.Find(id);
                    db.compra.Remove(compra);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult PrimerReporte()
        {
            try
            {
                var db = new inventario2021Entities();
                var query = from tabCompra in db.compra
                            join tabUsuario in db.usuario on tabCompra.id_usuario equals tabUsuario.id
                            join tabCliente in db.cliente on tabCompra.id_cliente equals tabCliente.id
   
                            select new PrimerReporte
                            {
                                nombreUsuario = tabUsuario.nombre,
                                apellidoUsuario = tabUsuario.apellido,
                                totalCompra = tabCompra.total,
                                fecha = tabCompra.fecha,
                                nombreCliente = tabCliente.nombre,
                                email = tabCliente.email
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
            return new ActionAsPdf("PrimerReporte") { FileName = "reporte.pdf" };
        }
    }
}