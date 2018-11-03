using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SQLite;
using Android.Util;

namespace Avalia_Pesquisa
{
    class DataBase
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public bool CriarBancoDeDados()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.CreateTable<Municipio>();
                    conexao.CreateTable<Localidade>();
                    conexao.CreateTable<Config>();
                  
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                #if __ANDROID__
                    Log.Info("SQLiteEx", ex.Message);
                #endif
                return false;
            }
        }

        public bool CheckInstalacao()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var dados = conexao.Query<Config>("SELECT * FROM Config Where Descricao=? and Valor=?", "carga_inicial", "1").ToList();
                    //conexao.Update(aluno);

                    if (dados.Count > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }

        }

   
    }
}
