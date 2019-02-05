using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Equipamento
    {
        [PrimaryKey]
        public int IdEquipamento { get; set; }
        public string Descricao { get; set; }
        public decimal Largura { get; set; }
        public int Bicos { get; set; }
        public decimal Volume_Calda { get; set; }
        public int Situacao { get; set; }
    }
}
