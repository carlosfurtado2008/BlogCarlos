using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blogcarlos.Web.Models.Administracao
{
    public class CadastrarPostViewModel
    {
        [DisplayName("Código")]
        public int Id { get;set; }

        [DisplayName("Título")]
        [Required(ErrorMessage = "* O campo Título é obrigatório.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "* A quantidade de caracteres no campo Título deve ser entre {2} e {1}.")]
        public string Titulo { get; set; }

        [DisplayName("Autor")]
        [Required(ErrorMessage = "* O campo Autor é obrigatório.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "* A quantidade de caracteres no campo Título deve ser entre {2} e {1}.")]
        public string Autor { get; set; }

        [DisplayName("Data Publicação")]
        [Required(ErrorMessage = "* O campo Data Publicação  é obrigatório.")]
        public DateTime DataPublicacao { get; set; }

        [DisplayName("Hora Publicação")]
        [Required(ErrorMessage = "* O campo Hora publicação é obrigatório.")]
        public DateTime HoraPublicacao { get; set; }

        [DisplayName("Descrição do Post")]
        [Required(ErrorMessage = "* O campo Descrição é obrigatório.")]
        public string Descricao { get; set; }

        [DisplayName("Resumo")]
        [Required(ErrorMessage = "* O campo Resumo é obrigatório.")]
        [StringLength(1000, MinimumLength = 2,
            ErrorMessage = "* A quantidade de caracteres no campo Título deve ser entre {2} e {1}.")]
        public string Resumo { get; set; }
        
        [DisplayName("Visivel")]
        [Required(ErrorMessage = "* O campo Visivel é obrigatório.")]
        public bool Visivel { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Comentarios { get; set; }
    }
}