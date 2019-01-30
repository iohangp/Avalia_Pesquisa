using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Plantio
    {
        [PrimaryKey][AutoIncrement]
        public int idPlantio { get; set; }
        public int idLocalidade { get; set; }
        public DateTime Data_Plantio { get; set; }
        public int idCultura { get; set; }
        public int idVariedade { get; set; }
        public int idSafra { get; set; }
        public DateTime? Data_Germinacao { get; set; }
        public int idGleba { get; set; }
        public int idUmidade_Solo { get; set; }
        public decimal Adubacao_Base { get; set; }
        public decimal Adubacao_Cobertura { get; set; }
        public decimal Espacamento { get; set; }
        public string Observacoes { get; set; }
        public decimal Populacao { get; set; }
        public int idUsuario { get; set; }
        public int idCulturaAnterior { get; set; }
        public int idSolo { get; set; }
        public int idCultura_Cobertura_Solo { get; set; }
        public decimal Metragem { get; set; }
        public int Status { get; set; }
        public int Integrado { get; set; }
    }
}
