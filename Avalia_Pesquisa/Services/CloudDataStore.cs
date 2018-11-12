using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Plugin.Connectivity;
using SQLite;

namespace Avalia_Pesquisa
{
    public class CloudDataStore : IDataStore<Item>
    {
        HttpClient client;
        IEnumerable<Item> items;
        IEnumerable<Usuario> userArray;
        IEnumerable<Municipio_Localidade> locArray;
        IEnumerable<Municipio> municipioArray;
        IEnumerable<Cultura> culturaArray;
        IEnumerable<Estudo> estudoArray;

        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public CloudDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

            items = new List<Item>();
        }

        public async Task<bool> BaixarCultura(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var json = await client.GetStringAsync($"cultura");
            culturaArray = JsonConvert.DeserializeObject<IEnumerable<Cultura>>(json);

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var cultura in culturaArray)
                    {
                        var cult = new Cultura
                        {
                            IdCultura = cultura.IdCultura,
                            Descricao = cultura.Descricao
                        };

                        var dadosCultura = conexao.Query<Cultura>("SELECT * FROM Cultura Where idCultura=?", cultura.IdCultura);

                        if (dadosCultura.Count == 0)
                            conexao.Insert(cult);
                        else
                        {
                            conexao.Query<Cultura>("UPDATE Cultura " +
                                                      "SET Descricao = ? " +
                                                    "WHERE IdCultura = ?",cult.Descricao,cult.IdCultura);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> BaixarEstudos(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var json = await client.GetStringAsync($"estudo");
            estudoArray = JsonConvert.DeserializeObject<IEnumerable<Estudo>>(json);

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var estudo in estudoArray)
                    {
                        var est = new Estudo
                        {
                            IdEstudo = estudo.IdEstudo,
                            Protocolo = estudo.Protocolo,
                            idCliente = estudo.idCliente,
                            Cliente = estudo.Cliente,
                            idEmpresa = estudo.idEmpresa,
                            Empresa = estudo.Empresa,
                            idCultura = estudo.idCultura,
                            idProduto = estudo.idProduto,
                            Produto = estudo.Produto,
                            idClasse = estudo.idClasse,
                            Classe = estudo.Classe,
                            idAlvo = estudo.idAlvo,
                            Alvo = estudo.Alvo,
                            Repeticao = estudo.Repeticao,
                            Intervalo_Aplicacao = estudo.Intervalo_Aplicacao,
                            Tratamento_Sementes = estudo.Tratamento_Sementes,
                            Aplicacoes = estudo.Aplicacoes,
                            Tratamentos = estudo.Tratamentos,
                            Volume_Calda = estudo.Volume_Calda,
                            Objetivo = estudo.Objetivo,
                            RET = estudo.RET,
                            Validade_RET = estudo.Validade_RET,
                            Observacoes = estudo.Observacoes,
                            idUsuario = estudo.idUsuario,
                            Data = estudo.Data,
                            idStatus = estudo.idStatus,
                            idResponsavel = estudo.idResponsavel,
                            RET_Fase = estudo.RET_Fase
                        };

                        var dadosEstudo = conexao.Query<Cultura>("SELECT * FROM Estudo Where idEstudo=?", est.IdEstudo);

                        if (dadosEstudo.Count == 0)
                            conexao.Insert(est);
                        else
                            conexao.Update(est);


                    }
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }

            return true;
        }


        public async Task<bool> LocalidadeSync(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var jsonLoc = await client.GetStringAsync($"localidade/localidade");
            locArray = JsonConvert.DeserializeObject<IEnumerable<Municipio_Localidade>>(jsonLoc);

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var local in locArray)
                    {
                        var dadosLocal = conexao.Query<Municipio_Localidade>("SELECT * FROM Municipio_Localidade Where idLocalidade=?", local.IdLocalidade);

                        if (dadosLocal.Count == 0)
                        {
                            conexao.Query<Municipio_Localidade>("INSERT INTO Municipio_Localidade (IdLocalidade,IdMunicipio,Descricao) Values(?,?,?)",
                                                  local.IdLocalidade, local.IdMunicipio, local.Descricao);
                        }
                        else
                        {
                            conexao.Query<Municipio_Localidade>("UPDATE Municipio_Localidade " +
                                                                    "SET Descricao = ?," +
                                                                        "IdMunicipio = ?" +
                                                                    "WHERE IdLocalidade = ?", local.Descricao, local.IdMunicipio, local.IdLocalidade);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> MunicipiosSync(string chave)
        {
            
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var jsonMunicipio = await client.GetStringAsync($"localidade/municipio");
            municipioArray = JsonConvert.DeserializeObject<IEnumerable<Municipio>>(jsonMunicipio);

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var elemento in municipioArray)
                    {
                        var dados = conexao.Query<Municipio>("SELECT * FROM Municipio Where IdMunicipio=?", elemento.IdMunicipio);

                        if(dados.Count == 0)
                        {
                            conexao.Query<Municipio>("INSERT INTO Municipio (IdMunicipio,Descricao) Values(?,?)", elemento.IdMunicipio, elemento.Descricao);
                        }
                        else
                        {
                            conexao.Query<Municipio>("UPDATE Municipio " +
                                                        "SET Descricao = ? " +
                                                        "WHERE IdMunicipio = ? ", elemento.Descricao, elemento.IdMunicipio);
                        }
                    }                
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }

            return true;
 
        }

        public async Task<bool> UsuarioSync(string chave)
        {

            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var json = await client.GetStringAsync($"users");

            userArray = JsonConvert.DeserializeObject<IEnumerable<Usuario>>(json);

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var elemento in userArray)
                    {
                        var dados = conexao.Query<Usuario>("SELECT * FROM Usuario Where IdUsuario=?", elemento.IdUsuario);

                        if (dados.Count == 0)
                        {
                            conexao.Query<Usuario>("INSERT INTO Usuario (IdUsuario,Nome,Cpf,Senha) Values(?,?,?,?)", 
                                                    elemento.IdUsuario, elemento.Nome, elemento.Cpf, elemento.Senha);
                        }
                        else
                        {
                            conexao.Query<Usuario>("UPDATE Usuario " +
                                                    "SET Senha = ?, Nome = ? " +
                                                    "WHERE IdUsuario = ?",
                                                    elemento.Senha,elemento.Nome,elemento.IdUsuario);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }

        return true;
        }

        public async Task<Item> GetItemAsync(string id)
        {
            if (id != null && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/item/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Item>(json));
            }

            return null;
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            if (item == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            if (item == null || item.Id == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/item/{item.Id}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !CrossConnectivity.Current.IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/item/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
