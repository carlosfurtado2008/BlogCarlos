using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BlogCarlos.DB.Classes;

namespace BlogCarlos.DB.Infra
{
    public class MeuCriadorDeBanco : DropCreateDatabaseAlways<ConexaoBanco>
    {
        protected override void Seed(ConexaoBanco context)
        {
            context.Usuarios.Add(new Usuario {Login =  "ADM", Nome = "Administrador", Senha = "admin" });
            base.Seed(context);
        }
    }
}
