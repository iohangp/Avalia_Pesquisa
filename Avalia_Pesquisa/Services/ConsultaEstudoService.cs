using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;


namespace Avalia_Pesquisa
{
    class ConsultaEstudoService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public List<Estudo> GetEstudo(string protocolo)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    return conexao.Query<Estudo>("SELECT e.*, c.Descricao as cultura FROM Estudo e" +
                                                  " JOIN Cultura c ON c.IdCultura = e.idCultura " +
                                                  " Where Protocolo=?", protocolo).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }

        }


    }
}
