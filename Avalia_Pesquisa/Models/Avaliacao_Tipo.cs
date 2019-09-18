using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Avaliacao_Tipo
    {
        [PrimaryKey]
        public int IdAvaliacao_Tipo { get; set; }
        public string Descricao { get; set; }
        public int idAvaliacao_Tipo_Config { get; set; }
    }
}
