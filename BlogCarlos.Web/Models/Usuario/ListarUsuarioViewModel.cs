using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogCarlos.Web.Models.Usuario
{
    public class ListarUsuarioViewModel
    {
        public List<BlogCarlos.DB.Classes.Usuario> Usuarios { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }

    }
}
 