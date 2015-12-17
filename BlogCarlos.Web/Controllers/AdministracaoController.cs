using Blogcarlos.Web.Models.Administracao;
using BlogCarlos.DB;
using BlogCarlos.DB.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogCarlos.Web.Controllers
{   [Authorize]
    public class AdministracaoController : Controller
    {
        //GET: Administracao teste
        public ActionResult Index()
        {
            return View();
        }
        #region Cadastrar Post
        public ActionResult CadastrarPost()
        {
            var viewModel = new CadastrarPostViewModel();
            viewModel.DataPublicacao = DateTime.Now;
            viewModel.HoraPublicacao = DateTime.Now;
            viewModel.Autor = "Carlos Cardoso Furtado Junior";
            return View(viewModel);
        }
   
        [HttpPost]
        public ActionResult CadastrarPost(CadastrarPostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var conexao = new ConexaoBanco();
                var postDados = new Post();

                /* uma forma de converter
                string dataHora;
                dataHora = viewModel.DataPublicacao.ToString("dd/MM/yyyy") + " " + viewModel.HoraPublicacao.ToString("hh:mm:ss");
                postDados.DataPublicacao = Convert.ToDateTime(dataHora);
                */
                postDados.Titulo = viewModel.Titulo;
                postDados.Autor = viewModel.Autor;
                postDados.DataPublicacao = new DateTime(viewModel.DataPublicacao.Year,
                                                        viewModel.DataPublicacao.Month,
                                                        viewModel.DataPublicacao.Day,
                                                        viewModel.HoraPublicacao.Hour,
                                                        viewModel.HoraPublicacao.Minute,
                                                        viewModel.HoraPublicacao.Second);
                postDados.Descricao = viewModel.Descricao;
                postDados.Resumo = viewModel.Resumo;
                postDados.Visivel = viewModel.Visivel;

                postDados.TagPost = new List<TagPost> ();

                if (viewModel.Tags != null)
                {
                    foreach (var item in viewModel.Tags)
                    {
                        var tagExiste = (from p in conexao.TagClass
                                         where p.Tag.ToLower() == item.ToLower()
                                         select p).Any();
                        if (!tagExiste)
                        {
                            var tagClass = new TagClass();
                            tagClass.Tag = item;
                            conexao.TagClass.Add(tagClass);
                        }
                        var postTag = new TagPost();
                        postTag.IdTag = item;
                        postDados.TagPost.Add(postTag);
                    }
                }

                conexao.Posts.Add(postDados);

                try
                {
                    conexao.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                }
            }
            return View(viewModel);
        }
        #endregion Cadastrar Post

        #region Editar Post
        //1 Criar Action "Editar Post"
        //2 receber id por parametro
        public ActionResult EditarPost(int id)
        {
            //2 Abrir conexao banco
            var conexao = new ConexaoBanco();
            

            /*var postDados = (from x in conexao.Posts
                         where x.Id == id
                         select x).FirstOrDefault();*/
             //3 Buscar Post no banco através do Id recebido
            var postDados = conexao.Posts.FirstOrDefault(x => x.Id == id);
            
            //4 Verificar se existe erro
            if (postDados == null)
            {
                throw new Exception(string.Format("Post com código {0} não encontrado.",id));
            }
                //5 Criar view model do tipo CadastrarPostViewModel
                var viewModel = new CadastrarPostViewModel();
                //6 Jogar os valors da classe posr na variavel viewmodel
                viewModel.Id = postDados.Id;
                viewModel.Titulo = postDados.Titulo;
                viewModel.Autor = postDados.Autor;
                viewModel.DataPublicacao = postDados.DataPublicacao;
                viewModel.HoraPublicacao = postDados.DataPublicacao;
                viewModel.Descricao = postDados.Descricao;
                viewModel.Resumo = postDados.Resumo;
                viewModel.Visivel = postDados.Visivel;
                viewModel.Tags = (from p in postDados.TagPost
                                    select p.IdTag).ToList();

            return View(viewModel);
        }

        // Colocar HHp Post em cima da Action
        [HttpPost]
        //Criar  action Editar post que recebe o viewmodel por parametro
        public ActionResult EditarPost(CadastrarPostViewModel viewModel)
        {
            // Validar o Modelo senão for válido não deve perder o que usuário digitou
            if (ModelState.IsValid)
            {    //  Abrir conexão com o banco
                var conexao = new ConexaoBanco();
                //Buscar o  Post no banco ue eu vou alterar
                var postDados = (from x in conexao.Posts
                         where x.Id == viewModel.Id
                         select x).FirstOrDefault();
                //Carregar os dados a ser alteradoss do viewmodel p o Post
                postDados.Titulo = viewModel.Titulo;
                postDados.Autor = viewModel.Autor;
                postDados.DataPublicacao = new DateTime(viewModel.DataPublicacao.Year,
                                                        viewModel.DataPublicacao.Month,
                                                        viewModel.DataPublicacao.Day,
                                                        viewModel.HoraPublicacao.Hour,
                                                        viewModel.HoraPublicacao.Minute,
                                                        viewModel.HoraPublicacao.Second);
                postDados.Descricao = viewModel.Descricao;
                postDados.Resumo = viewModel.Resumo;
                postDados.Visivel = viewModel.Visivel;
                postDados.TagPost = new List<TagPost>();

                var postsTagsAtuais = postDados.TagPost.ToList();
                foreach (var item in postsTagsAtuais)
                {
                    conexao.TagPosts.Remove(item);
                }
                

                if (viewModel.Tags != null)
                {
                    foreach (var item in viewModel.Tags)
                    {
                        var tagExiste = (from p in conexao.TagClass
                                         where p.Tag.ToLower() == item.ToLower()
                                         select p).Any();
                        if (!tagExiste)
                        {
                            var tagClass = new TagClass();
                            tagClass.Tag = item;
                            conexao.TagClass.Add(tagClass);
                        }
                        var postTag = new TagPost();
                        postTag.IdTag = item;
                        postDados.TagPost.Add(postTag);
                    }
                }
                //Salvar as alerações no banco
                try
                {
                    conexao.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                }
            }
            return View(viewModel);
        }
        #endregion Editar Post

        #region Excluir Post
        public ActionResult ExcluirPost(int id)
        {
            var conexao = new ConexaoBanco();
            var postDados = (from p in conexao.Posts
                        where p.Id == id
                        select p).FirstOrDefault();

            if (postDados == null)
            {
                throw new Exception(string.Format("Post com código {0} não existe.", id));
            }

            conexao.Posts.Remove(postDados);
            try
            {
                conexao.SaveChanges();
                            }
            catch (Exception exp)
            {
                ModelState.AddModelError("", exp.Message);
            }
            return RedirectToAction("Index", "Blog");
        }
        #endregion Excluir Post



        #region ExcluirComentario
        public ActionResult ExcluirComentario(int id)
        {
            var conexaoBanco = new ConexaoBanco();
            var comentario = (from p in conexaoBanco.Comentarios
                              where p.Id == id
                              select p).FirstOrDefault();
            if (comentario == null)
            {
                throw new Exception(string.Format("Comentário código {0} não foi encontrado.", id));
            }
            conexaoBanco.Comentarios.Remove(comentario);
            conexaoBanco.SaveChanges();

            var post = (from p in conexaoBanco.Posts
                        where p.Id == comentario.IdPost
                        select p).First();
            return Redirect(Url.Action("Post", "Blog", new
            {
                ano = post.DataPublicacao.Year,
                mes = post.DataPublicacao.Month,
                dia = post.DataPublicacao.Day,
                titulo = post.Titulo,
                id = post.Id
            }) + "#comentarios");
        }
        #endregion

    }
}