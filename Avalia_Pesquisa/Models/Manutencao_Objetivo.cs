using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Manutencao_Objetivo
    {
        [PrimaryKey]
        public int idManutencao_Objetivo { get; set; }
        public string Descricao { get; set; }
        public int Situacao { get; set; }

    }
}
