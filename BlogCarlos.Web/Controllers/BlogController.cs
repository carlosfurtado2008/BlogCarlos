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
                                  orderby p.DataPublicacao descending
                         select p);
            //1412215
            if (!string.IsNullOrEmpty(tag))
            {
                //prepara a consulta SQL
                posts = (from p in conexaoBanco.Posts
                         where p.TagPost.Any(x => x.IdTag.ToUpper() == tag.ToUpper())
                         orderby p.DataPublicacao descending
                         select p);
            }

            if (!string.IsNullOrEmpty(pesquisa))
            {
                //prepara a consulta SQL
                posts = (from p in conexaoBanco.Posts
                         where p.Titulo.ToUpper().Contains(pesquisa.ToUpper())
                            || p.Resumo.ToUpper().Contains(pesquisa.ToUpper())
                            || p.Descricao.ToUpper().Contains(pesquisa.ToUpper())
                         orderby p.DataPublicacao descending
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
                               select new DetalhesPostViewModel
                               {
                                   DataPublicacao = p.DataPublicacao,
                                   Autor = p.Autor,
                                   Descricao = p.Descricao,
                                   Id = p.Id,
                                   Resumo = p.Resumo,
                                   Titulo = p.Titulo,
                                   Visivel = p.Visivel,
                                   QtdeComentarios = p.Comentarios.Count
                               }).Skip(qtdeRegistrosPular).Take(registrosPorPagina).ToList();
            viewModel.PaginaAtual = paginaCorreta;
            viewModel.TotalPaginas = (int) qtdePaginas;
            //1412215
            viewModel.Tag = tag;
            viewModel.Tags = (from p in conexaoBanco.TagClass
                              where conexaoBanco.TagPosts.Any(x => x.IdTag == p.Tag)
                              orderby  p.Tag
                              select p).ToList();
            //viewModel.Posts = posts;
            viewModel.Pesquisa = pesquisa;
            return View(viewModel);
        }
        public ActionResult _Paginacao()
        {
            return PartialView();
        }

        #region Post
        public ActionResult Post(int id, int? pagina)
        {
            var conexao = new ConexaoBanco();

            var post = (from p in conexao.Posts
                        where p.Id == id
                        select p).FirstOrDefault();
            if (post == null)
            {
                throw new Exception(string.Format("Post codigo {0} não encontrado", id));
            }
            var viewModel = new DetalhesPostViewModel();
            preencherViewModel(post, viewModel,pagina);
            return View(viewModel);
        }
        //CTRL + R + M CRIA FUNCAO SELECIONANDO 
        private void preencherViewModel(Post post, DetalhesPostViewModel viewModel, int? pagina)
        {
            viewModel.Id = post.Id;
            viewModel.Titulo = post.Titulo;
            viewModel.Autor = post.Autor;
            viewModel.DataPublicacao = post.DataPublicacao;
            viewModel.HoraPublicacao = post.DataPublicacao;
            viewModel.Descricao = post.Descricao;
            viewModel.Resumo = post.Resumo;
            viewModel.Visivel = post.Visivel;
            viewModel.QtdeComentarios = post.Comentarios.Count;
            viewModel.Tags = post.TagPost.Select(x => x.IdTag).ToList();

            var paginaCorreta = pagina.GetValueOrDefault(1); 
            var registrosPorPagina = 5;
            var qtdeRegistros = post.Comentarios.Count;
            var indiceDaPagina = paginaCorreta - 1;
            var qtdeRegistrosPular = (indiceDaPagina * registrosPorPagina);
            var qtdePaginas = Math.Ceiling((decimal)qtdeRegistros / registrosPorPagina);
            viewModel.Comentarios = (from p in post.Comentarios
                                     orderby p.DataHora descending
                                     select p).Skip(qtdeRegistrosPular).Take(registrosPorPagina).ToList();
            viewModel.PaginaAtual = paginaCorreta;
            viewModel.TotalPaginas = (int) qtdePaginas;
        }
        #endregion

        [HttpPost]
        public ActionResult Post(DetalhesPostViewModel viewModel)
        {
            var conexaoBanco = new ConexaoBanco();
            var post = (from p in conexaoBanco.Posts
                        where p.Id == viewModel.Id
                        select p).FirstOrDefault();

            if (ModelState.IsValid)
            { 
                if (post == null)
                {
                    throw new Exception(string.Format("Post código {0} não encontrado.", viewModel.Id));
                }
                var comentario = new Comentario();
                comentario.AdmPost = HttpContext.User.Identity.IsAuthenticated;
                comentario.Descricao = viewModel.ComentarioDescricao;
                comentario.Email = viewModel.ComentarioEmail;
                comentario.IdPost = viewModel.Id;
                comentario.Nome = viewModel.ComentarioNome;
                comentario.PaginaWeb = viewModel.ComentarioPaginaWeb;
                comentario.DataHora = DateTime.Now;
                try
                {
                    conexaoBanco.Comentarios.Add(comentario);
                    conexaoBanco.SaveChanges();
                    return Redirect(Url.Action("Post", new
                    {
                        ano = post.DataPublicacao.Year,
                        mes = post.DataPublicacao.Month,
                        dia = post.DataPublicacao.Day,
                        titulo = post.Titulo,
                        id = post.Id
                    }) + "#comentarios");
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                }
            }
            preencherViewModel(post, viewModel, null);
            return View(viewModel);

        }
        public ActionResult _PaginacaoPost()
        {
            return PartialView();
        }

    }
}

   