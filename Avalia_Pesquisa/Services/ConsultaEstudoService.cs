﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;


namespace Avalia_Pesquisa
{
    class ConsultaEstudoService
    {
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public List<ViewEstudo> GetEstudo(string protocolo)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<ViewEstudo>("SELECT e.*, c.Descricao as Cultura FROM Estudo e" +
                                                  " INNER JOIN Cultura c ON c.IdCultura = e.IdCultura " +
                                                  " Where Protocolo=?", protocolo).ToList();

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