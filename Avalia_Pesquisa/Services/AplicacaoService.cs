using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;
using System;

namespace Avalia_Pesquisa
{
    class AplicacaoService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public bool SalvarAplicacao(Aplicacao aplicacao)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.Insert(aplicacao);

                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }

        }


        public List<Estudo_Planejamento> GetDataAplicacao(int idEstudo)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Estudo_Planejamento>("SELECT ep.idEstudo_Planejamento, ep.idEstudo, ep.data " +
                                                                     "FROM Estudo_Planejamento ep " +
                                                                     "JOIN Estudo_Tipo_Alvo eta ON ep.idEstudo = eta.idEstudo " +
                                                                     "WHERE ep.idEstudo = ? " +
                                                                     "and not exists(SELECT 1 FROM Avaliacao a " +
                                                                                    "WHERE a.idEstudo_Planejamento = ep.idEstudo_Planejamento " +
                                                                                    "AND a.idAvaliacao_Tipo = eta.idAvaliacao_Tipo " +
                                                                                    "AND a.idAlvo = eta.idAlvo) " +
                                                                     "AND ep.tipo = 1 " +
                                                                     "GROUP by ep.idEstudo_Planejamento " +
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