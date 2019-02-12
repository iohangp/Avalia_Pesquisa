using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Produto
    {
        [PrimaryKey]
        public int idProdutos { get; set; }
        public string Descricao { get; set; }
        public int situacao { get; set; }
    }
}
