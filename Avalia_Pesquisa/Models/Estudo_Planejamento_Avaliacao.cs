﻿using SQLite;
using System;

namespace Avalia_Pesquisa
{
    public class Estudo_Planejamento_Avaliacao
    {
        [PrimaryKey]
        public int idEstudo_Planejamento_Avaliacao { get; set; }
        public int idEstudo { get; set; }
        public int Num_Avaliacao { get; set; }
        public DateTime? data { get; set; }
        public int idAvaliacao_Tipo { get; set; }
        public int idAlvo { get; set; }
        public int? idEstudo_Planejamento_Avaliacao_Web { get; set; }
        public int Integrado { get; set; }
    }
}
