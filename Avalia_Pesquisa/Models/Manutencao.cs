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
        public int idUnidade_Medida { get; set; }
        public int idManutencao_Objetivo { get; set; }
        public DateTime Hora_Inicio_Fim { get; set; }
        public string Temperatura { get; set; }
        public decimal Umidade_Relativa { get; set; }
        public decimal Velocidade_Vento { get; set; }
        public decimal Percentual_Nuvens { get; set; }
        public string Observacoes { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}