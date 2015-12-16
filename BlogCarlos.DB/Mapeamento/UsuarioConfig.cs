using BlogCarlos.DB.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCarlos.DB.Mapeamento
{
    public class UsuarioConfig : EntityTypeConfiguration<Usuario>
    {   //cto tab tab
        public UsuarioConfig()
        {
            ToTable("USUARIO");

            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("IDUSUARIO")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Login)
                .HasColumnName("LOGIN")
                .HasMaxLength(30)
                .IsRequired();

            Property(x => x.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(100)
                .IsRequired();

            Property(x => x.Senha)
                .HasColumnName("SENHA")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
