using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  
using SQLite;
using System;

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

        public List<Umidade_Solo> GetUmidades()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Umidade_Solo>("SELECT * FROM Umidade_Solo").ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<Solo> GetSolos()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Solo>("SELECT * FROM Solo").ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<Cobertura_Solo> GetCoberturas()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    var result = conexao.Query<Cobertura_Solo>("SELECT * FROM Cobertura_Solo").ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<Municipio_Localidade> GetLocalidades()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    //  var result = conexao.Query<ViewPlantio>("SELECT p.idPlantio, CONCAT(l.Descricao,' - ',s.Descricao, ' - ', g.Descricao) as Descricao from plantio p join municipio_localidade l on l.idLocalidade = p.idLocalidade join gleba g on g.idGleba = p.idGleba join safra s on s.idSafra = p.idSafra where p.idCultura = ?").ToList();

                    var result = conexao.Query<Municipio_Localidade>("SELECT l.idLocalidade, m.Descricao || ' - ' || l.Descricao as descricao "+
                                                                    "FROM Municipio m "+
                                                                    "JOIN Municipio_Localidade l on l.idMunicipio = m.idMunicipio").ToList();

                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public bool SalvarPlantio(Plantio plan)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.Insert(plan);

                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
