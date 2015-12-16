using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogCarlos.Web.Models.Blog
{
    public class DetalhesPostViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public DateTime DataPublicacao { get; set; }
        public DateTime HoraPublicacao { get; set; }
        public string Descricao { get; set; }
        public string Resumo { get; set; }
        public bool Visivel { get; set; }
        public List<string> Tags { get; set; }
    }
}