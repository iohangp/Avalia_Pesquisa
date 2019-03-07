using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Avaliacao_Imagem
    {
        [PrimaryKey][AutoIncrement]
        public int IdAvaliacao_Imagem { get; set; }
        public string Imagem { get; set; }
        public byte[] ImagemByte { get; set; }
        public int idAvaliacao { get; set; }
        public int Tratamento { get; set; }
        public int Repeticao { get; set; }
        public int idUsuario { get; set; }
        public DateTime Data { get; set; }
        public int? idAvaliacao_ImagemWeb { get; set; }
    }
}
