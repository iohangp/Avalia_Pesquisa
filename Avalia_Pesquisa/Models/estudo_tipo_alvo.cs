using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Estudo_Tipo_Alvo
    {
        [PrimaryKey][AutoIncrement]
        public int idEstudoTipoAlvo { get; set; }
        public int idAvaliacao_tipo { get; set; }
        public int IdAlvo { get; set; }
        public int IdEstudo { get; set; }
    }
}
