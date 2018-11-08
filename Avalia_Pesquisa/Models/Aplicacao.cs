﻿using System;

namespace Avalia_Pesquisa
{
    public class Aplicacao
    {
        public int IdAplicacao { get; set; }
        public int idInstalacao { get; set; }
        public DateTime Data_Aplicacao { get; set; }
        public decimal Dosagem { get; set; }
        public decimal Umidade_Relativa { get; set; }
        public decimal Temperatura { get; set; }
        public decimal Velocidade_Vento { get; set; }
        public decimal Percentual_nuvens { get; set; }
        public DateTime Chuva_Data { get; set; }
        public decimal Chuva_Volume { get; set; }
        public int IdEquipamento { get; set; }
        public decimal BBCH { get; set; }
        public string Observacoes { get; set; }
    }
}
