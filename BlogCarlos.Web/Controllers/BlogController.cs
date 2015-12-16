using BlogCarlos.DB;
using BlogCarlos.DB.Classes;
using BlogCarlos.Web.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogCarlos.Web.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        //1412215
        public ActionResult Index(int? pagina, string tag, string pesquisa) //Passar por parametro pagina pode ser nula?
        {
            var paginaCorreta = pagina.GetValueOrDefault(1); //A primeira vez que entrar no controller informar 1
            var registrosPorPagina = 5;

            //p.Visivel = verdadeiro
            //!p.Visivel igual a false

            var conexaoBanco = new ConexaoBanco();
            //List<Post> posts = (from p in conexaoBanco.Posts
            //                    where p.Visivel == true
            //                    orderby p.DataPublicacao descending
            //                    select p).ToList();

            var posts = (from p in conexaoBanco.Posts
                                where p.Visivel == true
//1412215                                orderby p.DataPublicacao descending
                         select p);
            //1412215
            if (!string.IsNullOrEmpty(tag))
            {
                posts = (from p in posts
                         where p.TagPost.Any(x => x.IdTag.ToUpper() == tag.ToUpper())
                         select p);

            }

            if (!string.IsNullOrEmpty(pesquisa))
            {
                posts = (from p in posts
                         where p.Titulo.ToUpper().Contains(pesquisa.ToUpper())
                            || p.Resumo.ToUpper().Contains(pesquisa.ToUpper())
                            || p.Descricao.ToUpper().Contains(pesquisa.ToUpper())
                         select p);
            }

            var qtdeRegistros = posts.Count();
            var indiceDaPagina = paginaCorreta - 1;
            var qtdeRegistrosPular = (indiceDaPagina * registrosPorPagina);
            var qtdePaginas = Math.Ceiling((decimal)qtdeRegistros / registrosPorPagina);

            var viewModel = new ListarPostsViewModel();
            //1412215  
            viewModel.Posts = (from p in posts
                               orderby p.DataPublicacao descending
                               select p).Skip(qtdeRegistrosPular).Take(registrosPorPagina).ToList();
            viewModel.PaginaAtual = paginaCorreta;
            viewModel.TotalPaginas = (int) qtdePaginas;
            //1412215
            viewModel.Tag = tag;
            viewModel.Tags = (from p in conexaoBanco.TagClass
                              where conexaoBanco.TagPosts.Any(x => x.IdTag == p.Tag)
                              orderby  p.Tag
                              select p.Tag).ToList();
            //viewModel.Posts = posts;
            viewModel.Pesquisa = pesquisa;
            return View(viewModel);
        }
        public ActionResult _Paginacao()
        {
            return PartialView();
        }

        #region Post
        public ActionResult Post(int id)
        {
            var conexao = new ConexaoBanco();

            var postDados = (from x in conexao.Posts
                             where x.Id == id
                             select new DetalhesPostViewModel
                             {
                                 Id = x.Id,
                                 Titulo = x.Titulo,
                                 Autor = x.Autor,
                                 DataPublicacao = x.DataPublicacao,
                                 HoraPublicacao = x.DataPublicacao,
                                 Descricao = x.Descricao,
                                 Resumo = x.Resumo,
                                 Visivel = x.Visivel,
                                 Tags = x.TagPost.Select(p => p.IdTag).ToList()

                             }).FirstOrDefault();


            return View(postDados);
        }
        #endregion

    }
}

