using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogCarlos.Web.Models.Usuario
{
    public class CadastrarUsuarioViewModel
    {
        [DisplayName("Código")]
        public int Id { get; set; }

        [DisplayName("Login")]
        [StringLength(30, MinimumLength = 2,
            ErrorMessage = "* A quantidade de caracteres no campo Login deve ser entre {2} e {1}.")]
        [Required(ErrorMessage = "* O campo Login é obrigatório.")]
        public string Login { get; set; }

        [DisplayName("Nome")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "* A quantidade de caracteres no campo Nome deve ser entre {2} e {1}.")]
        [Required(ErrorMessage = "* O campo Login é obrigatório.")]
        public string Nome { get; set; }
        
        [DisplayName("Senha")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "* A quantidade de caracteres no campo Senha deve ser entre {2} e {1}.")]
        [Required(ErrorMessage = "* O campo Senha é obrigatório.")]
        public string Senha { get; set; }

        [DisplayName("Confirmar Senha")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "* A quantidade de caracteres no campo Confirmar Senha deve ser entre {2} e {1}.")]
        [Required(ErrorMessage = "* O campo Confirmar Senha é obrigatório.")]
        [Compare("Senha",ErrorMessage = "As senhas digitadas não conferem!")]
        public string ConfirmarSenha { get; set; }

    }
}
