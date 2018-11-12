using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Estudo
    {
        [PrimaryKey]
        public int IdEstudo { get; set; }
        public string Protocolo { get; set; }
        public int idCliente { get; set; }
        public string Cliente { get; set; }
        public int idEmpresa { get; set; }
        public string Empresa { get; set; }
        public int idCultura { get; set; }
        public int idProduto { get; set; }
        public string Produto { get; set; }
        public int idClasse { get; set; }
        public string Classe { get; set; }
        public int idAlvo { get; set; }
        public string Alvo { get; set; }
        public int Repeticao { get; set; }
        public int Intervalo_Aplicacao { get; set; }
        public int Tratamento_Sementes { get; set; }
        public int Aplicacoes { get; set; }
        public int Tratamentos { get; set; }
        public decimal Volume_Calda { get; set; }
        public string Objetivo { get; set; }
        public string RET { get; set; }
        public DateTime Validade_RET { get; set; }
        public string Observacoes { get; set; }
        public int idUsuario { get; set; }
        public DateTime Data { get; set; }
        public int idStatus { get; set; }
        public int RET_Fase { get; set; }
        public int idResponsavel { get; set; }
    }
}
