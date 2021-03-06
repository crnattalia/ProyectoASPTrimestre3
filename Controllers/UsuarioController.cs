using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using ProyectoASPTrimestre3.Models;
using System.Web.Security;
using System.Text;
using System.Web.Routing;


namespace ProyectoASPTrimestre3.Controllers
{
    public class UsuarioController : Controller
    {
        [Authorize]
        // GET: Usuario
        //Consultar tabla - retorna todos los usuarios
        //public ActionResult Index()
        //{
        //    using (var db = new inventario2021Entities()) 
        //    {
        //     return View(db.usuario.ToList());
        //    }
            
        //}

        //Mostrar formulario registrar un usuario
        public ActionResult Create()
        {
            return View();
        }

        //Método para recibir la información de Create
        //Registrar Usuario y guardar usuario 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(usuario usuario)
        {
            if (!ModelState.IsValid)
                return View();
        
            try
            {
                using (var db = new inventario2021Entities())
                {
                    usuario.password = UsuarioController.HashSHA1(usuario.password);
                    db.usuario.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return RedirectToAction("View"); 
            }
        }

        //Encriptar la contraseña
        public static string HashSHA1(string value)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for(var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            
            return sb.ToString();
        }

        //Detalles de usuario
        public ActionResult Details(int id)
        {
            using (var db = new inventario2021Entities())
            {
                var findUser = db.usuario.Find(id);
                return View(findUser);
            }
        }

        //Eliminar
        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var findUser = db.usuario.Find(id);
                    db.usuario.Remove(findUser);
                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }

        }

        //Editar
        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var findUser = db.usuario.Where(a => a.id == id).FirstOrDefault();
                    return View(findUser); 
                }

            }catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        //Recibir datos de editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(usuario editUser)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    usuario user = db.usuario.Find(editUser.id);
                    user.nombre = editUser.nombre;
                    user.apellido = editUser.apellido;
                    user.email = editUser.email;
                    user.fecha_nacimiento = editUser.fecha_nacimiento;
                    user.password = editUser.password;

                    db.SaveChanges();
                    return RedirectToAction("PaginadorIndex"); 

                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }

        public ActionResult Login(string mensaje = "")
        {
            ViewBag.Message = mensaje;
            return View();
        }

        //Metodo para recibir los datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string user, string password)
        {
            try
            {
                string passEncrip = UsuarioController.HashSHA1(password);
                using (var db = new inventario2021Entities())
                {
                    var userLogin = db.usuario.FirstOrDefault(e => e.email == user && e.password == passEncrip);
                    if (userLogin != null)
                    {
                        FormsAuthentication.SetAuthCookie(userLogin.email, true);
                        Session["user"] = userLogin;
                        return RedirectToAction("PaginadorIndex");
                    }
                    else
                    {
                        return Login("Verifique sus datos"); 
                    }
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError(" ", "Error " + ex);
                return View();
            }
        }

        [Authorize]
        public ActionResult CloseSession()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult uploadCSV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult uploadCSV(HttpPostedFileBase fileForm)
        {
            try
            {
                string filePath = string.Empty;

                if (fileForm != null)
                {
                    string path = Server.MapPath("~/Uploads/");

                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(fileForm.FileName);

                    string extension = Path.GetExtension(fileForm.FileName);

                    fileForm.SaveAs(filePath);

                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach (string row in csvData.Split('\n'))
                    {
                        if(!string.IsNullOrEmpty(row))
                        {
                            var newUsuario = new usuario
                            {
                                nombre = row.Split(';')[0],
                                apellido = row.Split(';')[1],
                                fecha_nacimiento = Convert.ToDateTime(row.Split(';')[2]),
                                email = row.Split(';')[3],
                                password = row.Split(';')[4]
                            };

                            using (var db = new inventario2021Entities())
                            {
                                db.usuario.Add(newUsuario);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                return View();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex);
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
                    var usuarios = db.usuario.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistros).Take(cantidadRegistros).ToList();

                    var totalRegistros = db.usuario.Count();
                    var modelo = new ModeloIndex();
                    modelo.Usuarios = usuarios;
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