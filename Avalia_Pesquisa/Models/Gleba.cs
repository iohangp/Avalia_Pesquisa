using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Gleba
    {
        [PrimaryKey]
        public int idGleba { get; set; }
        public string Descricao { get; set; }
        public decimal Metragem { get; set; }
        public int Ativo { get; set; }

    }
}
