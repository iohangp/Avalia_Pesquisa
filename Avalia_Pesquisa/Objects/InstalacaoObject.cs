using System;

namespace Avalia_Pesquisa
{
    public class InstalacaoObject
    {
        public int idInstalacao { get; set; }
        public int idEstudo { get; set; }
        public int idPlantio { get; set; }
        public decimal Tamanho_Parcela_Comprimento { get; set; }
        public decimal Tamanho_Parcela_Largura { get; set; }
        public string Coordenadas1 { get; set; }
        public string Coordenadas2 { get; set; }
        public string Altitude { get; set; }
        public DateTime Data_Instalacao { get; set; }
        public int idUsuario { get; set; }
        public string Observacoes { get; set; }
        public int idStatus { get; set; }
        public Plantio Dados_Plantio { get; set; }
    }
}
