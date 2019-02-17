using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;


namespace Avalia_Pesquisa
{
    class ConsultaEstudoService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public List<ViewEstudo> GetEstudo(string codigo)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<ViewEstudo>("SELECT e.*, i.idInstalacao, c.Descricao as Cultura FROM Estudo e" +
                                                  " INNER JOIN Cultura c ON c.IdCultura = e.IdCultura " +
                                                  " LEFT JOIN Instalacao i ON e.idEstudo = i.idEstudo " +
                                                  " Where e.Codigo=? GROUP BY e.idEstudo", codigo).ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }

        }


    }
}
