﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;
using System;

namespace Avalia_Pesquisa
{
    class AvaliacaoService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public bool SalvarAvaliacao(Avaliacao avaliacao)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.Insert(avaliacao);

                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }

        }

        public List<Alvo> GetAlvos(int idTipoAvaliacao, int idEstudo)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Alvo>("SELECT a.idAlvo, a.Nome_vulgar, a.Especie FROM Alvo a " +
                                                    "JOIN Estudo_Tipo_Alvo e ON e.idAlvo = a.idAlvo "+
                                                    "where e.idEstudo = ? AND e.idAvaliacao_tipo = ? ",idEstudo, idTipoAvaliacao).ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<Estudo_Planejamento> GetDataAvaliacao(int idEstudo)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Estudo_Planejamento>("SELECT ep.idEstudo_Planejamento, ep.idEstudo, ep.data "+
                                                                     "FROM estudo_planejamento ep "+
                                                                     "WHERE ep.idEstudo = ? "+
                                                                     "and not exists(SELECT 1 FROM avaliacao a "+
                                                                     "WHERE a.idEstudo_Planejamento = ep.idEstudo_Planejamento) "+
                                                                     "AND ep.tipo = 1 "+
                                                                     "ORDER BY ep.data asc limit 1; ", idEstudo).ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


    }
}
