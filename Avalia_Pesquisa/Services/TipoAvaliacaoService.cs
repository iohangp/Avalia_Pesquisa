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

        public List<Avaliacao_Tipo> GetAvaliacaoTipo(int idEstudo, int idPlanejamento)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    /* var result = conexao.Query<Avaliacao_Tipo>("SELECT a.idAvaliacao_Tipo, Descricao from Avaliacao_Tipo a "+
                                                                 "JOIN Estudo_Tipo_Alvo ata ON ata.idAvaliacao_tipo = a.idAvaliacao_tipo " +
                                                                 "LEFT JOIN estudo_planejamento ep ON ep.idEstudo = ata.idEstudo " +
                                                                 "WHERE ata.idEstudo = ? AND ep.idEstudo_Planejamento = ? " +
                                                                 "AND not exists (SELECT 1 FROM avaliacao a2 "+
                                                                                  "WHERE a2.idEstudo_Planejamento = ep.idEstudo_Planejamento "+
                                                                                  "AND a2.idAvaliacao_Tipo = ata.idAvaliacao_Tipo "+
                                                                                  "AND a2.idAlvo = ata.idAlvo) "+
                                                                 "GROUP BY ata.idAvaliacao_tipo; ", idEstudo, idPlanejamento).ToList();
                     */

                    var result = conexao.Query<Avaliacao_Tipo>(" SELECT a.idAvaliacao_Tipo, Descricao from Avaliacao_Tipo a "+
                                                                " JOIN Estudo_Tipo_Alvo ata ON ata.idAvaliacao_tipo = a.idAvaliacao_tipo " +
                                                                " LEFT JOIN Estudo_Planejamento ep ON ep.idEstudo = ata.idEstudo " +
                                                                " WHERE ata.idEstudo = ? "+
                                                                " GROUP BY ata.idAvaliacao_tipo; ", idEstudo).ToList();
                   
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
