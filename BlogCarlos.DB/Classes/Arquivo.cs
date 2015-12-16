using BlogCarlos.DB.Classes.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCarlos.DB.Classes
{
    public class Arquivo : ClasseBase
    {
        public string Nome { get; set; }
        public string Extensao { get; set; }
        public Byte[] Bytes { get; set; }
        public int IdPost { get; set; }

        //public object IdPost { get; internal set; }
        public virtual Post Post { get; set; }
        
    }
}
