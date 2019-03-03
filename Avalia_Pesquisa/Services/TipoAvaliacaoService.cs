using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;
using System;

namespace Avalia_Pesquisa
{
    class TipoAvaliacaoService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public List<Avaliacao_Tipo> GetAvaliacaoTipo(int idEstudo, int Tratamento)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                     var result = conexao.Query<Avaliacao_Tipo>("SELECT a.idAvaliacao_Tipo, Descricao" +
                                                                " FROM Avaliacao_Tipo a "+
                                                                 "JOIN Estudo_Tipo_Alvo ata ON ata.idAvaliacao_tipo = a.idAvaliacao_tipo " +
                                                                 "JOIN Estudo_Planejamento_Avaliacao ep ON ep.idEstudo = ata.idEstudo " +
                                                                 "AND ep.idAlvo = ata.idAlvo AND ep.idAvaliacao_Tipo = ata.idAvaliacao_Tipo " +
                                                                 "WHERE ata.idEstudo = ? " + // AND ep.Num_Avaliacao = ?
                                                                 "AND not exists (SELECT 1 FROM avaliacao a2 " +
                                                                                  "WHERE a2.idEstudo_Planejamento = ep.idEstudo_Planejamento_Avaliacao " +
                                                                                  "AND a2.idAvaliacao_Tipo = ata.idAvaliacao_Tipo "+
                                                                                  "AND a2.idAlvo = ata.idAlvo " +
                                                                                  "AND a2.Tratamento = ?) " +
                                                                 "GROUP BY ata.idAvaliacao_tipo; ", idEstudo, Tratamento).ToList();

                      
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
