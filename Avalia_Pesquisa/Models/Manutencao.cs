using System;

namespace Avalia_Pesquisa
{
    public class Manutencao
    {
        public int idManutencao { get; set; }
        public int idInstalacao { get; set; }
        public int idManutencao_Tipo { get; set; }
        public DateTime Data { get; set; }
        public int idProduto { get; set; }
        public int idUsuario { get; set; }
        public decimal Dose { get; set; }
        public int idObjetivo { get; set; }
        public DateTime Hora_Inicio_Fim { get; set; }
        public decimal Temperatura { get; set; }
        public decimal Umidade_Relativa { get; set; }
        public decimal Velocidade_Vento { get; set; }
        public decimal Percentual_Nuves { get; set; }
        public string Observacoes { get; set; }
    }
}
