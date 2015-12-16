using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogCarlos.Web.Models.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "* O campo Login é obrigatório.")]
        [Display(Name ="Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "* O campo Senha é obrigatório.")]
        [Display(Name ="Senha")]
        public string Senha { get; set; }

        [Display(Name = "Lembrar?")]
        public bool Lembrar { get; set; }
    }
}