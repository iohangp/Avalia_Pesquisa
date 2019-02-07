using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Manutencao_Tipo
    {
        [PrimaryKey]
        public int idManutencao_Tipo { get; set; }
        public string Descricao { get; set; }
        public int Situacao { get; set; }
    }
}
