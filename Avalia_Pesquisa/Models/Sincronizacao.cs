using System;

namespace Avalia_Pesquisa
{
    public class Sincronizacao
    {
        public int idSincronizacao { get; set; }
        public int idTabela { get; set; }
        public int idIndice { get; set; }
        public int idAparelho { get; set; }
        public int Banco_Sistema { get; set; }
        public int Banco_Aplicativo { get; set; }
        public int idUsuario { get; set; }
        public DateTime data { get; set; }
    }
}
