using BlogCarlos.DB.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogCarlos.Web.Models.Blog
{
    public class ListarPostsViewModel
    {
        public List<DetalhesPostViewModel> Posts { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas{ get; set; }
        //1412215
        public string Tag { get; set; }
        public List<string> Tags { get; set; }
        public string Pesquisa { get; set; }

    }
}