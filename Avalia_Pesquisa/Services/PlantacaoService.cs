﻿using System.Collections.Generic;
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

        public List<Gleba> GetGlebas()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Gleba>("SELECT * FROM Gleba").ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }

        }

        public List<Safra> GetSafras()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Safra>("SELECT * FROM Safra").ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }

        }


        public List<ViewPlantio> GetPlantio()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                   //  var result = conexao.Query<ViewPlantio>("SELECT p.idPlantio, CONCAT(l.Descricao,' - ',s.Descricao, ' - ', g.Descricao) as Descricao from plantio p join municipio_localidade l on l.idLocalidade = p.idLocalidade join gleba g on g.idGleba = p.idGleba join safra s on s.idSafra = p.idSafra where p.idCultura = ?").ToList();

                    var result = conexao.Query<ViewPlantio>("SELECT * from plantio").ToList();

                    //var result = conexao.Query<ViewPlantio>("1").ToList();

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
