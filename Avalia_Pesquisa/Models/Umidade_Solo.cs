using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Umidade_Solo
    {
        [PrimaryKey]
        public int idUmidade_Solo { get; set; }
        public string Descricao { get; set; }

    }
}
