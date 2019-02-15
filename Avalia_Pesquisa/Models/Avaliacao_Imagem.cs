using System;

namespace Avalia_Pesquisa
{
    public class Avaliacao_Imagem
    {
        public int IdAvaliacao_Imagem { get; set; }
        public string Imagem { get; set; }
        public int idAvaliacao { get; set; }
        public int tratamento { get; set; }
        public int repeticao { get; set; }
        public int idUsuario { get; set; }
        public DateTime Data { get; set; }
        public int idAvaliacao_ImagemWeb { get; set; }
    }
}
