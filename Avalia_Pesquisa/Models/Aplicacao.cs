﻿using System;
using SQLite;

namespace Avalia_Pesquisa
{
    public class Aplicacao
    {
        [PrimaryKey][AutoIncrement]
        public int IdAplicacao { get; set; }
        public int idInstalacao { get; set; }
        public DateTime Data_Aplicacao { get; set; }
        public DateTime Data_Realizada { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public decimal Dosagem { get; set; }
        public decimal Umidade_Relativa { get; set; }
        public string Temperatura { get; set; }
        public decimal Velocidade_Vento { get; set; }
        public decimal Percentual_Nuvens { get; set; }
        public DateTime? Chuva_Data { get; set; }
        public decimal Chuva_Volume { get; set; }
        public int idEquipamento { get; set; }
        public decimal BBCH { get; set; }
        public string Observacoes { get; set; }
        public int idUsuario { get; set; }
        public int? idEstudo_Planejamento { get; set; }
        public int? idAplicacaoWeb { get; set; }
    }
}
