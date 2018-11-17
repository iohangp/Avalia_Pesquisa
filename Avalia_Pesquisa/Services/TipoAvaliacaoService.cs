using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;


namespace Avalia_Pesquisa
{
    class TipoAvaliacaoService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public List<Avaliacao_Tipo> GetAvaliacaoTipo()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Avaliacao_Tipo>("SELECT * FROM Avaliacao_Tipo").ToList();

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
