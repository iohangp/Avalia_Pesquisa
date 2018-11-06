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
        CloudDataStore CloudData;

        public bool CriarBancoDeDados()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.CreateTable<Municipio>();
                    conexao.CreateTable<Localidade>();
                    conexao.CreateTable<Config>();
                    conexao.CreateTable<Usuario>();
                  
                    return true;
                }
            }
            catch (SQLiteException ex)
            {

                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool CheckInstalacao()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    conexao.Query<Config>("DELETE FROM Config Where Descricao=? and Valor=?", "carga_inicial", "1");

                    var dados = conexao.Query<Config>("SELECT * FROM Config Where Descricao=? and Valor=?", "carga_inicial", "1").ToList();

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

        public bool InserirConfig(Config config)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.Query<Config>("DELETE FROM Config Where Descricao=? and Valor=?", config.Descricao, config.Valor);

                    conexao.Query<Config>("INSERT INTO Config (Descricao,Valor) Values(?,?)", config.Descricao, config.Valor);
                    return true;
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
