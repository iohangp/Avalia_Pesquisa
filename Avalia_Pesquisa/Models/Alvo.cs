using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Alvo
    {
        [PrimaryKey]
        public int IdAlvo { get; set; }
        public string Especie { get; set; }
        public string Nome_vulgar { get; set; }
    }
}
