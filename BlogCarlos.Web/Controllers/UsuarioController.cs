using BlogCarlos.DB;
using BlogCarlos.DB.Classes;
using BlogCarlos.Web.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogCarlos.Web.Controllers
{   [Authorize]
    public class UsuarioController : Controller
    {
        //Lista os usuários
        public ActionResult Index(int? pagina)
        {
            //abre a conexão do banco
            var conexaoBanco = new ConexaoBanco();
            //Controle de paginação
            var registrosPorPagina = 3;
            //se é nulo assume 1
            var paginaCorreta = pagina.GetValueOrDefault(1);
            //indice da página
            var indiceDaPagina = paginaCorreta - 1;
            //indica a quantidade de registros que precisamos pular
            var qtdeRegistrosPular = (indiceDaPagina * registrosPorPagina);

            //prepara a consulta SQL
            var dados = (from p in conexaoBanco.Usuarios
                         where p.Login != "ADM"
                         orderby p.Nome descending
                         select p);
            //retorna a quantidade de registros
            var qtdeRegistros = dados.Count();
            //calcula o número de páginas
            var numeroPaginas = Math.Ceiling((Decimal)qtdeRegistros / registrosPorPagina);
            //instancia o modelo
            var viewModel = new ListarUsuarioViewModel();
            //retorna somente os registros selecionados
            viewModel.Usuarios = dados.Skip(qtdeRegistrosPular).Take(registrosPorPagina).ToList();
            viewModel.PaginaAtual = paginaCorreta;
            viewModel.TotalPaginas = (int)numeroPaginas;

            return View(viewModel);
        }
  

        #region Cadastrar Usuario
        public ActionResult CadastrarUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CadastrarUsuario(CadastrarUsuarioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var conexao = new ConexaoBanco();
                
                try
                {
                    var jaexiste = (from p in conexao.Usuarios
                                    where p.Login.ToUpper() == viewModel.Login.ToUpper()
                                    select p).Any();
                    if (jaexiste)
                    {
                        throw new Exception(string.Format("Já existe usuário cadastrado com o Login {0}.", viewModel.Login));
                    }

                    var usuarioDados = new Usuario();

                    usuarioDados.Login = viewModel.Login;
                    usuarioDados.Nome = viewModel.Nome;
                    usuarioDados.Senha = viewModel.Senha;

                    conexao.Usuarios.Add(usuarioDados);
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
        #endregion Cadastrar Usuario


        public ActionResult EditarUsuario(int id)
        {
            var conexao = new ConexaoBanco();

            //var postDados = conexao.Posts.FirstOrDefault(x => x.Id == id);
            var dados = (from x in conexao.Usuarios where x.Id == id select x).FirstOrDefault();

            if (dados == null)
            {
                throw new Exception(string.Format("Usuário com código {0} não encontrado.", id));
            }

            var viewModel = new CadastrarUsuarioViewModel();
            viewModel.Id = dados.Id;
            viewModel.Login = dados.Login;
            viewModel.Nome = dados.Nome;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditarUsuario(CadastrarUsuarioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var conexao = new ConexaoBanco();

                //var postDados = conexao.Posts.FirstOrDefault(x => x.Id == id);
                var dados = (from x in conexao.Usuarios where x.Id == viewModel.Id select x).FirstOrDefault();

                if (dados == null)
                {
                    throw new Exception(string.Format("Usuário com código {0} não encontrado.", viewModel.Id));
                }

                dados.Id = viewModel.Id;
                dados.Login = viewModel.Login.ToUpper();
                dados.Nome = viewModel.Nome;
                dados.Senha = viewModel.Senha.ToLower();

                try
                {
                    conexao.SaveChanges();
                    //redireciona para o Index do controller atual
                    return RedirectToAction("Index");
                    //redireciona para o Index do controller Administracao
                    //return RedirectToAction("Index", "Administracao");
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);

                }
            }
            return View(viewModel);
        }

        #region Excluir Usuario
        public ActionResult ExcluirUsuario(int? id)
        {
            var conexao = new ConexaoBanco();
            var usuarioDados = (from p in conexao.Usuarios
                             where p.Id == id
                             select p).FirstOrDefault();

            if (usuarioDados == null)
            {
                throw new Exception(string.Format("Usuário com código {0} não existe.", id));
            }

            conexao.Usuarios.Remove(usuarioDados);
            try
            {
                conexao.SaveChanges();
            }
            catch (Exception exp)
            {
                ModelState.AddModelError("", exp.Message);
            }
            return RedirectToAction("Index", "Usuario");
        }
        #endregion Excluir Post


        public ActionResult _Paginacao()
        {
            return PartialView();
        }
    }

}