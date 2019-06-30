using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class BBCH_Estagio
    {
        [PrimaryKey]
        public int IdBBCH_Estagio { get; set; }
        public int idCultura { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
    }
}
