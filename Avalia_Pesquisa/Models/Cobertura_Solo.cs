using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Cobertura_Solo
    {
        [PrimaryKey]
        public int idCobertura_Solo { get; set; }
        public string Descricao { get; set; }

    }
}
