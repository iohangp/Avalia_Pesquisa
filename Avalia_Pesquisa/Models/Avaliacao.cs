using System;

namespace Avalia_Pesquisa
{
    public class Avaliacao
    {
        public int IdAvaliacao { get; set; }
        public int idInstalacao { get; set; }
        public DateTime Data { get; set; }
        public int idUsuario { get; set; }
        public int idAvaliacao_Tipo { get; set; }
        public decimal Valor { get; set; }
        public string Observacao { get; set; }
        public int idAlvo { get; set; }
    }
}
