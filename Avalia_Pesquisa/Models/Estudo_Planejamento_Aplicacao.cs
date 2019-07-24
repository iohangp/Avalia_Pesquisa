using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Estudo_Planejamento_Aplicacao
    {
        [PrimaryKey]
        public int idEstudo_Planejamento_Aplicacao { get; set; }
        public int idEstudo { get; set; }
        public int Num_Aplicacao { get; set; }
        public DateTime? data { get; set; }
        public int Integrado { get; set; }
    }
}
