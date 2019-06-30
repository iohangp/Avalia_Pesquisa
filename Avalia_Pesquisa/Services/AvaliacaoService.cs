using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;
using System;
using System.Dynamic;

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

        public List<BBCH> GetBBCH()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {


                    var result = conexao.Query<BBCH>("SELECT idBBCH, b.codigo, c.descricao as cultura, c.idCultura, idBbch_estagio, b.descricao FROM bbch b join cultura c on c.idCultura = b.idCultura ");

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<BBCH_Estagio> GetBBCH_estagio()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {


                    var result = conexao.Query<BBCH_Estagio>("select idBbch_estagio, codigo, be.descricao, c.idCultura, c.descricao as cultura from bbch_estagio be join cultura c on c.idCultura = be.idCultura ");

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<Alvo> GetAlvos(int idTipoAvaliacao, int idEstudo)
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

        public List<Estudo_Planejamento_Avaliacao> GetPlanejamentoAlvo(int idEstudo, int Tratamento, int idAvaliacao_Tipo, int idAlvo)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Estudo_Planejamento_Avaliacao>("SELECT ep.idEstudo_Planejamento_Avaliacao, ep.idEstudo, ep.data, ep.Num_Avaliacao " +
                                                                      "FROM Estudo_Planejamento_Avaliacao ep " +
                                                                      "WHERE ep.idEstudo = ? AND ep.idAvaliacao_Tipo = ? AND ep.idAlvo = ? " +
                                                                      "and not exists(SELECT 1 FROM Avaliacao a " +
                                                                                     "WHERE a.idEstudo_Planejamento = ep.idEstudo_Planejamento_Avaliacao " +
                                                                                     "AND a.idAvaliacao_Tipo = ep.idAvaliacao_Tipo " +
                                                                                     "AND a.idAlvo = ep.idAlvo " +
                                                                                     "AND a.Tratamento = ?) " +
                                                                      "GROUP by ep.idEstudo_Planejamento_Avaliacao " +
                                                                      "ORDER BY ep.data asc LIMIT 1;",idEstudo, idAvaliacao_Tipo, idAlvo, Tratamento).ToList();


                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<Estudo_Planejamento_Avaliacao> GetDataAvaliacao(int idEstudo, int Tratamento)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                   var result = conexao.Query<Estudo_Planejamento_Avaliacao>("SELECT ep.idEstudo_Planejamento_Avaliacao, ep.idEstudo, ep.data, ep.Num_Avaliacao " +
                                                                     "FROM Estudo_Planejamento_Avaliacao ep " +
                                                                     "JOIN Estudo_Tipo_Alvo eta ON ep.idEstudo = eta.idEstudo " +
                                                                     "AND ep.idAvaliacao_Tipo = eta.idAvaliacao_Tipo AND eta.idAlvo = ep.idAlvo " +
                                                                     "WHERE ep.idEstudo = ? "+
                                                                     "and not exists(SELECT 1 FROM Avaliacao a "+
                                                                                    "WHERE a.idEstudo_Planejamento = ep.idEstudo_Planejamento_Avaliacao " +
                                                                                    "AND a.idAvaliacao_Tipo = eta.idAvaliacao_Tipo " +
                                                                                    "AND a.idAlvo = eta.idAlvo " +
                                                                                    "AND a.Tratamento = ?) " +
                                                                     "GROUP by ep.idEstudo_Planejamento_Avaliacao " +
                                                                     "ORDER BY ep.data asc, ep.Num_Avaliacao LIMIT 1;", idEstudo, Tratamento).ToList();

                  
                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public dynamic GetPlanejamentoEstudo(int idEstudo, int idAlvo, int idAvaliacao_Tipo, int Num_Avaliacao)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<ViewDataAvaliacao>("SELECT epa.data, ap.Num_Avaliacao, ap.Dias, ap.Apos, " +
                                                                        " CASE ap.idTipoPlanejamento "+
                                                                                 " WHEN 1 THEN 'A' "+
                                                                                 " WHEN 2 THEN 'AV' "+
                                                                                 " WHEN 5 THEN 'G' "+
                                                                                 " ELSE '' "+
                                                                                 " END as Sigla, ap.idTipoPlanejamento " +
                                                                    "FROM Avaliacao_Planejamento ap "+
                                                                   " JOIN Estudo_Planejamento_Avaliacao epa ON epa.idEstudo = ap.idEstudo "+
                                                                     "AND epa.idAlvo = ap.idAlvo AND epa.idAvaliacao_Tipo = ap.idAvaliacao_Tipo "+
                                                                     "AND epa.Num_Avaliacao = ap.Num_Avaliacao "+
                                                                  " WHERE ap.idEstudo = ? AND ap.idAlvo = ? AND ap.idAvaliacao_Tipo = ? "+
                                                                   "  AND ap.Num_Avaliacao = ? "+
                                                                  " GROUP BY idEstudo_Planejamento_Avaliacao LIMIT 1; ", idEstudo, idAlvo, idAvaliacao_Tipo, Num_Avaliacao).ToList();

                    dynamic planEstudo = new ExpandoObject();

                    string numAval = "";
                    if (result[0].idTipoPlanejamento == 3)
                        numAval = "Prévia";
                    else if (result[0].idTipoPlanejamento == 4)
                        numAval = "Colheita";
                    else
                        numAval = result[0].Dias.ToString() + "DA" + result[0].Apos.ToString() + result[0].Sigla.ToString();

                    planEstudo.numAval = numAval;
                    planEstudo.dataAval = result[0].data.ToString("dd/MM/yyyy");

                    return planEstudo;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public bool GerarPlanejamentoAvaliacao(int idEstudo, DateTime data)
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

                            DateTime dataAplic = data;

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
                                    dataAplic = data;

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
                                    dataAplic = data;
                            }
                            else
                            {
                                dataAplic = data;
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
