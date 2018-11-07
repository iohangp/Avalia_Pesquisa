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
        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public CloudDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

            items = new List<Item>();
        }

        public bool MunicipiosSync(string chave)
        {
            
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            List<Municipio> municipio = new List<Municipio>
            {
                new Municipio(){ IdMunicipio =1,Descricao="Ponta Grossa" },
                new Municipio(){ IdMunicipio =2,Descricao="Curitiba" },
            };
            var serializedMunicipio = JsonConvert.SerializeObject(municipio);

            List<Municipio_Localidade> municipio_localidade = new List<Municipio_Localidade>
            {
                new Municipio_Localidade(){ IdLocalidade =1,Descricao="Cará-Cará",IdMunicipio=1},
                new Municipio_Localidade(){ IdLocalidade =2,Descricao="Água Verde",IdMunicipio=2 },
            };
            var serializedLocal = JsonConvert.SerializeObject(municipio_localidade);

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var elemento in municipio)
                    {
                        var dados = conexao.Query<Municipio>("SELECT * FROM Municipio Where IdMunicipio=?", elemento.IdMunicipio);

                        if(dados.Count == 0)
                        {
                            conexao.Query<Municipio>("INSERT INTO Municipio (IdMunicipio,Descricao) Values(?,?)", elemento.IdMunicipio, elemento.Descricao);
                        }
                    }

                    foreach (var local in municipio_localidade)
                    {
                        var dadosLocal = conexao.Query<Municipio_Localidade>("SELECT * FROM Municipio_Localidade Where idLocalidade=?", local.IdLocalidade);

                        if (dadosLocal.Count == 0)
                        {
                            conexao.Query<Municipio_Localidade>("INSERT INTO Municipio_Localidade (IdLocalidade,IdMunicipio,Descricao) Values(?,?,?)",
                                                  local.IdLocalidade, local.IdMunicipio, local.Descricao);
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

        public bool UsuarioSync(string chave)
        {
<<<<<<< Updated upstream
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            List<Usuario> usuario = new List<Usuario>
            {
                new Usuario(){ IdUsuario =1,Nome="Iohan Pierdoná",Cpf="07547555926",Senha="123456" },
                new Usuario(){ IdUsuario =2,Nome="Pedro Barros",Cpf="05404781998",Senha="123456" },
                new Usuario(){ IdUsuario =2,Nome="Michael bereza",Cpf="05892131998",Senha="1" },
            };

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var elemento in usuario)
                    {
                        var dados = conexao.Query<Usuario>("SELECT * FROM Usuario Where IdUsuario=?", elemento.IdUsuario);

                        if (dados.Count == 0)
                        {
                            conexao.Query<Usuario>("INSERT INTO Usuario (IdUsuario,Nome,Cpf,Senha) Values(?,?,?,?)", 
                                                    elemento.IdUsuario, elemento.Nome, elemento.Cpf, elemento.Senha);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }
=======
               if (!CrossConnectivity.Current.IsConnected)
            return false;

        List<Usuario> usuario = new List<Usuario>
        {
            new Usuario(){ IdUsuario =1,Nome="Iohan Pierdoná",Cpf="07547555926",Senha="123456" },
            new Usuario(){ IdUsuario =2,Nome="Pedro Barros",Cpf="05404781998",Senha="123456" },
            new Usuario(){ IdUsuario =3,Nome="Michael bereza",Cpf="05892131998",Senha="1" },
        };

        try
        {
            using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
           {
                foreach (var elemento in usuario)
                {
                    var dados = conexao.Query<Config>("SELECT * FROM Usuario Where IdUsuario=?", elemento.IdUsuario);

                    if (dados.Count == 0)
                    {
                       conexao.Query<Config>("INSERT INTO Usuario (IdUsuario,Nome,Cpf,Senha) Values(?,?,?,?)", 
                                                elemento.IdUsuario, elemento.Nome, elemento.Cpf, elemento.Senha);
                    }
                }
            }
        }
        catch (SQLiteException ex)
        {
            return false;
        }
>>>>>>> Stashed changes


        return true;
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/item");
                items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Item>>(json));
            }

            return items;
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
