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
    public class RolesController : Controller
    {
        // GET: Cliente
        //Consultar tabla - retorna todos los usuarios
        //public ActionResult Index()
        //{
        //    using (var db = new inventario2021Entities())
        //    {
        //        return View(db.roles.ToList());
        //    }
        //}

        //Registrar rol
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(roles roles)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.roles.Add(roles);
                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return RedirectToAction("View");
            }
        }

        //Detalles del rol
        public ActionResult Details(int id)
        {
            using (var db = new inventario2021Entities())
            {
                var findUser = db.roles.Find(id);
                return View(findUser);
            }
        }

        //Eliminar registro
        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var findUser = db.roles.Find(id);
                    db.roles.Remove(findUser);
                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        //Editar rol
        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var findUser = db.roles.Where(a => a.id == id).FirstOrDefault();
                    return View(findUser);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        //Recibir y guardar datos editados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(roles editRoles)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    roles roles = db.roles.Find(editRoles.id);
                    roles.descripcion = editRoles.descripcion;

                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
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
                    var roles = db.roles.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistros).Take(cantidadRegistros).ToList();

                    var totalRegistros = db.roles.Count();
                    var modelo = new ModeloIndex();
                    modelo.Roles = roles;
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
