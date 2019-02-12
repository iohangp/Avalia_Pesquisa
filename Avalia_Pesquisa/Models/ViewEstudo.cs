using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class ViewEstudo
    {
        public int IdEstudo { get; set; }
        public string Protocolo { get; set; }
        public string Cliente { get; set; }
        public string Empresa { get; set; }
        public string Cultura { get; set; }
        public string Produto { get; set; }
        public string Classe { get; set; }
        public string Alvo { get; set; }
        public string Codigo { get; set; }
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
        public string Usuario { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public int RET_Fase { get; set; }
        public string Responsavel { get; set; }
    }
}
