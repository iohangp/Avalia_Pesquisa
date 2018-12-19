using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Avaliacao
    {
        [PrimaryKey][AutoIncrement]
        public int IdAvaliacao { get; set; }
     //   public int idInstalacao { get; set; }
        public int IdEstudo { get; set; }
        public DateTime Data { get; set; }
        public int idUsuario { get; set; }
        public int idAvaliacao_Tipo { get; set; }
        public decimal Valor { get; set; }
        public string Observacao { get; set; }
        public int Repeticao { get; set; }
        public int idEstudo_Planejamento { get; set; }
        public int idAlvo { get; set; }
        public int Integrado { get; set; }
    }
}
