using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Cultura_Variedade
    {
        [PrimaryKey]
        public int IdVariedade { get; set; }
        public string Descricao { get; set; }
        public int idCultura { get; set; }
    }
}
