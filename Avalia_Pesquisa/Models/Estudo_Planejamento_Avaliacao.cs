using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Estudo_Planejamento_Avaliacao
    {
        [PrimaryKey][AutoIncrement]
        public int idEstudo_Planejamento_Avaliacao { get; set; }
        public int idEstudo { get; set; }
        public int Num_Avaliacao { get; set; }
        public DateTime data { get; set; }
        public int idAvaliacao_Tipo { get; set; }
        public int idAlvo { get; set; }
    }
}
