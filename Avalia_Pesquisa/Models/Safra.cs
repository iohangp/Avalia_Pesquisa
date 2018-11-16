using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Safra
    {
        [PrimaryKey]
        public int IdSafra { get; set; }
        public String Descricao { get; set; }

    }
}
