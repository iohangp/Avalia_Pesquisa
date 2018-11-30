using System;

namespace Avalia_Pesquisa
{
    public class ViewPlantio
    {
        public int idPlantio { get; set; }
        public int idLocalidade { get; set; }
        public DateTime Data_Plantio { get; set; }
        public int idCultura { get; set; }
        public int idVariedade { get; set; }
        public int idSafra { get; set; }
        public int idGleba { get; set; }
        public decimal Espacamento { get; set; }
        public string Observacoes { get; set; }
        public decimal Metragem { get; set; }
        public int Status { get; set; }
        public string Descricao { get; set; }
    }
}
