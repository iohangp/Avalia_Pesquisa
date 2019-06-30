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
                    conexao.CreateTable<Municipio_Localidade>();
                    conexao.CreateTable<Config>();
                    conexao.CreateTable<Usuario>();
                    conexao.CreateTable<Alvo>();
                    conexao.CreateTable<Aparelho>();
                    conexao.CreateTable<Aparelho_Modelo>();
                    conexao.CreateTable<Aplicacao>();
                    conexao.CreateTable<Avaliacao>(); 
                    conexao.CreateTable<Avaliacao_Imagem>();
                    conexao.CreateTable<Avaliacao_Tipo>();
                    conexao.CreateTable<Classe>();
                    conexao.CreateTable<Cliente>();
                    conexao.CreateTable<Cultura>();
                    conexao.CreateTable<Cultura_Variedade>();
                    conexao.CreateTable<Empresa>();
                    conexao.CreateTable<Equipamento>();
                    conexao.CreateTable<Estudo>();
                    conexao.CreateTable<Gleba>();
                    conexao.CreateTable<Instalacao>();
                    conexao.CreateTable<Item>();
                    conexao.CreateTable<Manutencao>();
                    conexao.CreateTable<Manutencao_Tipo>();
                    conexao.CreateTable<Manutencao_Objetivo>();
                    conexao.CreateTable<Objetivos>();
                    conexao.CreateTable<Plantio>();
                    conexao.CreateTable<Produto>();
                    conexao.CreateTable<Safra>();
                    conexao.CreateTable<Solo>();
                    conexao.CreateTable<Status>();
                    conexao.CreateTable<Umidade_Solo>();
                    conexao.CreateTable<Estudo_Tipo_Alvo>();
                    conexao.CreateTable<Estudo_Planejamento>();
                    conexao.CreateTable<Estudo_Planejamento_Aplicacao>();
                    conexao.CreateTable<Estudo_Planejamento_Avaliacao>();
                    conexao.CreateTable<Cobertura_Solo>();
                    conexao.CreateTable<Aplicacao_Planejamento>();
                    conexao.CreateTable<Avaliacao_Planejamento>();
                    conexao.CreateTable<Unidade_Medida>();
                    conexao.CreateTable<BBCH_Estagio>();
                    conexao.CreateTable<BBCH>();

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

                   // conexao.Query<Config>("DELETE FROM Config Where Descricao=? and Valor=?", "carga_inicial", "1");

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
                   // conexao.Query<Config>("DELETE FROM Config Where Descricao=? and Valor=?", config.Descricao, config.Valor);

                    conexao.Query<Config>("INSERT INTO Config (Descricao,Valor) Values(?,?)", config.Descricao, config.Valor);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
               // Log.Info("SQLiteEx", ex.Message);
                return false;
            }

        }


    }
}
