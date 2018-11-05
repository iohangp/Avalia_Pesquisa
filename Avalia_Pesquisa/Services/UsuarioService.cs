using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SQLite;
using Android.Util;

namespace Avalia_Pesquisa
{
    class UsuarioService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public List<Usuario> GetUsuario(string login, string senha)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    return conexao.Query<Usuario>("SELECT * FROM Usuario Where Cpf=? and Senha=?", login, senha).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }

        }


    }
}
