using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Config
    {
        [PrimaryKey][AutoIncrement]
        public int IdConfig { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }

    }
}
