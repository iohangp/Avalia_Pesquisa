using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Estudo_Tipo_Alvo
    {
        [PrimaryKey]
        public int IdEstudo_tipo_avaliacao_alvo { get; set; }
        public int idAvaliacao_tipo { get; set; }
        public int IdAlvo { get; set; }
        public int IdEstudo { get; set; }
    }
}
