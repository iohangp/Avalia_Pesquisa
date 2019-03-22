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

        public bool UpdateAplicacao(Aplicacao aplicacao)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.Update(aplicacao);

                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }

        }


        public Aplicacao LastID()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Aplicacao>("SELECT MAX(IdAplicacao) AS IdAplicacao FROM Aplicacao").ToList();

                    return result[0];
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
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

        public List<Estudo_Planejamento_Aplicacao> GetPlanejamentoAplic(int idEstudo)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Estudo_Planejamento_Aplicacao>("SELECT * FROM estudo_planejamento_aplicacao epa "+
                                                                               " WHERE idEstudo = ? " +
                                                                               "   AND NOT EXISTS(SELECT 1 FROM aplicacao a "+
                                                                                      " WHERE epa.idEstudo_Planejamento_Aplicacao = a.idEstudo_Planejamento) " +
                                                                               "ORDER BY Num_Aplicacao LIMIT 1;", idEstudo).ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<Equipamento> GetEquipamento()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Equipamento>("SELECT * from equipamento").ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public bool GerarPlanejamentoAplicacao(int idEstudo, DateTime data)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Aplicacao_Planejamento>("SELECT * from Aplicacao_Planejamento WHERE idEstudo = ?", idEstudo).ToList();
                  
                    DateTime dataAplic = data;

                    foreach (var res in result)
                    {                    
                        var resPlan = conexao.Query<Estudo_Planejamento_Aplicacao>("SELECT * from Estudo_Planejamento_Aplicacao " +
                                                                                    "WHERE idEstudo = ? AND Num_Aplicacao = ?", idEstudo, res.Num_Aplicacao).ToList();
                        if(resPlan.Count == 0) {
                            
                            if (res.Dias_Aplicacao != 0)
                                dataAplic = dataAplic.AddDays(res.Dias_Aplicacao);
                            else
                                dataAplic = data;

                            
                            var estPlan = new Estudo_Planejamento_Aplicacao
                            {
                                idEstudo = idEstudo,
                                Num_Aplicacao = res.Num_Aplicacao,
                                data = dataAplic
                            };

                            conexao.Insert(estPlan);
                        }

                    }
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
       
        }

    }
}