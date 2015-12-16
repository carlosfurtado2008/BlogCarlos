using BlogCarlos.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogCarlos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var conexao = new ConexaoBanco();
            var usuarios = (from u in conexao.Usuarios
                            select u).ToList();
            
            ViewBag.Message = "Blog Zenthi Sistemas.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "carlos.furtado@zenthi.com.br";

            return View();
        }
    }
}