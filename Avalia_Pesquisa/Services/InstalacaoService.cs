using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;


namespace Avalia_Pesquisa
{
    class InstalacaoService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public List<Instalacao> GetInstalacao()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Instalacao>("SELECT * FROM instalacao").ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }

        }

        public bool SalvarInstalacao(Instalacao instalacao)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.Insert(instalacao);

                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }

        }



    }
}
