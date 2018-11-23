using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Solo
    {
        [PrimaryKey]
        public int idSolo { get; set; }
        public string Descricao { get; set; }

    }
}
