using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Estudo_Planejamento_Avaliacao
    {
        [PrimaryKey]
        public int idEstudo_Planejamento_Avaliacao { get; set; }
        public int idEstudo { get; set; }
        public DateTime data { get; set; }
    }
}
