using System.Collections.Generic;
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

        public bool SalvarAvaliacaoImagem(Avaliacao_Imagem avaliacaoimagem)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.Insert(avaliacaoimagem);

                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }

        }

        public List<Avaliacao> GetUltimaAvaliacao()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {


                    var result = conexao.Query<Avaliacao>("SELECT idAvaliacao from avaliacao ORDER BY idAvaliacao DESC LIMIT 1");

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<Avaliacao_Imagem> GetImagem()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {


                    var result = conexao.Query<Avaliacao_Imagem>("SELECT imagem from avaliacao_imagem ORDER BY idAvaliacao DESC LIMIT 1");

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<Alvo> GetAlvos(int idTipoAvaliacao, int idEstudo, int idPlanejamento)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    /* var result = conexao.Query<Alvo>("SELECT a.idAlvo, a.Nome_vulgar, a.Especie FROM Alvo a " +
                                                        "JOIN Estudo_Tipo_Alvo e ON e.idAlvo = a.idAlvo " +
                                                        "JOIN estudo_planejamento ep ON ep.idEstudo = e.idEstudo " +
                                                       "WHERE e.idEstudo = ? AND e.idAvaliacao_tipo = ? AND ep.idEstudo_Planejamento = ?" +
                                                         "AND not exists (SELECT 1 FROM avaliacao a2 "+
                                                                           "WHERE a2.idEstudo_Planejamento = ep.idEstudo_Planejamento "+
                                                                           "AND a2.idAvaliacao_Tipo = e.idAvaliacao_Tipo "+
                                                                           "AND a2.idAlvo = e.idAlvo)" +
                                                       "GROUP BY a.idAlvo;", idEstudo, idTipoAvaliacao, idPlanejamento).ToList(); */

                    var result = conexao.Query<Alvo>("SELECT a.idAlvo, a.Nome_vulgar, a.Especie FROM Alvo a " +
                                                        "JOIN Estudo_Tipo_Alvo e ON e.idAlvo = a.idAlvo " +
                                                        "LEFT JOIN estudo_planejamento ep ON ep.idEstudo = e.idEstudo " +
                                                       "WHERE e.idEstudo = ? AND e.idAvaliacao_tipo = ? " +
                                                       "GROUP BY a.idAlvo;", idEstudo, idTipoAvaliacao).ToList();

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
                                                                     "FROM Estudo_Planejamento ep " +
                                                                     "JOIN Estudo_Tipo_Alvo eta ON ep.idEstudo = eta.idEstudo " +
                                                                     "WHERE ep.idEstudo = ? "+
                                                                     "and not exists(SELECT 1 FROM Avaliacao a "+
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

        public bool GerarPlanejamentoAvaliacao(int idEstudo)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Avaliacao_Planejamento>("SELECT * from Avaliacao_Planejamento WHERE idEstudo = ?", idEstudo).ToList();

                    foreach (var res in result)
                    {

                        var resPlan = conexao.Query<Estudo_Planejamento_Avaliacao>("SELECT * from Estudo_Planejamento_Avaliacao " +
                                                                                    "WHERE idEstudo = ? AND Num_Avaliacao = ? " +
                                                                                    "AND idAvaliacao_Tipo = ? AND idAlvo = ?", 
                                                                                    idEstudo, res.Num_Avaliacao, res.idAvaliacao_Tipo, res.idAlvo).ToList();

                        if (resPlan.Count == 0)
                        {

                            DateTime dataAplic = DateTime.Now;
                            
                            if (res.idTipoPlanejamento == 1)
                            {

                                var resAplic = conexao.Query<Estudo_Planejamento_Aplicacao>("SELECT * from Estudo_Planejamento_Aplicacao " +
                                                                                   "WHERE idEstudo = ? AND Num_Aplicacao = ? ",
                                                                                   idEstudo, res.Apos).ToList();
                                if (resAplic.Count > 0)
                                {
                                    dataAplic = resAplic[0].data.AddDays(res.Dias);
                                }
                                else
                                    dataAplic = DateTime.Now;

                            }
                            else if (res.idTipoPlanejamento == 2)
                            {
                                var resAval = conexao.Query<Estudo_Planejamento_Avaliacao>("SELECT * from Estudo_Planejamento_Avaliacao " +
                                                                                    "WHERE idEstudo = ? AND Num_Avaliacao = ? " +
                                                                                    "AND idAvaliacao_Tipo = ? AND idAlvo = ?",
                                                                                    idEstudo, res.Apos, res.idAvaliacao_Tipo, res.idAlvo).ToList();
                                if (resAval.Count > 0)
                                {
                                    dataAplic = resAval[0].data.AddDays(res.Dias);
                                }
                                else
                                    dataAplic = DateTime.Now;
                            }
                            else
                            {
                                dataAplic = DateTime.Now;
                            }


                            var estPlan = new Estudo_Planejamento_Avaliacao
                            {
                                idEstudo = idEstudo,
                                Num_Avaliacao = res.Num_Avaliacao,
                                data = dataAplic,
                                idAvaliacao_Tipo = res.idAvaliacao_Tipo,
                                idAlvo = res.idAlvo
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
