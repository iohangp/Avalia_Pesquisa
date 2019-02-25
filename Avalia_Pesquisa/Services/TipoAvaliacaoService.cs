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

        public List<Avaliacao_Tipo> GetAvaliacaoTipo(int idEstudo, string dataPlan, int Tratamento)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                     var result = conexao.Query<Avaliacao_Tipo>("SELECT a.idAvaliacao_Tipo, Descricao, strftime('%Y-%m-%d',ep.data) as datewhere" +
                                                                " FROM Avaliacao_Tipo a "+
                                                                 "JOIN Estudo_Tipo_Alvo ata ON ata.idAvaliacao_tipo = a.idAvaliacao_tipo " +
                                                                 "JOIN Estudo_Planejamento_Avaliacao ep ON ep.idEstudo = ata.idEstudo " +
                                                                 "WHERE ata.idEstudo = ? " +
                                                                 "AND not exists (SELECT 1 FROM avaliacao a2 "+
                                                                                  "WHERE a2.idEstudo_Planejamento = ep.idEstudo_Planejamento_Avaliacao " +
                                                                                  "AND a2.idAvaliacao_Tipo = ata.idAvaliacao_Tipo "+
                                                                                  "AND a2.idAlvo = ata.idAlvo " +
                                                                                  "AND a2.Tratamento = ?) " +
                                                                 "GROUP BY ata.idAvaliacao_tipo; ", idEstudo, Tratamento).ToList();

                    //  AND strftime('%Y-%m-%d',ep.data) = '"+ dataPlan + "'
                   /*    var result2 = conexao.Query<Estudo_Planejamento_Avaliacao>("SELECT DATE('now') as data " +
                                                                " FROM Avaliacao_Tipo a " +
                                                                 " JOIN Estudo_Tipo_Alvo ata ON ata.idAvaliacao_tipo = a.idAvaliacao_tipo " +
                                                                 " JOIN Estudo_Planejamento_Avaliacao ep ON ep.idEstudo = ata.idEstudo " +
                                                                 "WHERE ata.idEstudo = ? ", idEstudo).ToList();
                     */ 
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
