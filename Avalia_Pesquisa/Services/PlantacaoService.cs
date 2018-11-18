using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;


namespace Avalia_Pesquisa
{
    class PlantacaoService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public List<Cultura> GetCulturas()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Cultura>("SELECT * FROM Cultura").ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }

        }

        public List<Cultura_Variedade> GetVariedades(int idcultura)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Cultura_Variedade>("SELECT * FROM Cultura_Variedade WHERE idCultura = ?", idcultura).ToList();

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
