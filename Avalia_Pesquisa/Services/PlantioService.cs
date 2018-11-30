using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SQLite;




namespace Avalia_Pesquisa.Services
{
    class PlantacaoService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public List<Cultura> GetPlantio()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Plantio>("SELECT * FROM Plantio").ToList();

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
