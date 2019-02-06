using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Unidade_Medida
    {
        [PrimaryKey]
        public int idUnidade_Medida { get; set; }
        public string Descricao { get; set; }

    }
}
