using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Estudo_Planejamento
    {
        [PrimaryKey]
        public int idEstudo_planejamento { get; set; }
        public int idEstudo { get; set; }
        public DateTime data { get; set; }
        public int tipo { get; set; }
    }
}
