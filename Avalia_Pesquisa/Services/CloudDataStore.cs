using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        IEnumerable<Cultura_Variedade> variedadeArray;
        IEnumerable<Avaliacao_Tipo> tipoAvalArray;
        IEnumerable<Safra> safraArray;
        IEnumerable<Umidade_Solo> umidadeArray;
        IEnumerable<Gleba> glebaArray;
        IEnumerable<Solo> soloArray;
        IEnumerable<Alvo> alvoArray;
        IEnumerable<Estudo_Tipo_Alvo> tipoAlvoArray;

        string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public CloudDataStore()
        {
            client = new HttpClient();
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            }
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

            items = new List<Item>();
        }

        public async Task<bool> BaixarCultura(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/cultura");
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                culturaArray = JsonConvert.DeserializeObject<IEnumerable<Cultura>>(json);

                int total = culturaArray.Count();
                if (total == 0)
                    return false;

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
                                                        "WHERE IdCultura = ?", cult.Descricao, cult.IdCultura);
                            }
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

            }
            else
                return false;

            return true;
        }

        public async Task<bool> BaixarEstudos(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/estudo");
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                estudoArray = JsonConvert.DeserializeObject<IEnumerable<Estudo>>(json);

                int total = estudoArray.Count();
                if (total == 0)
                    return false;

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
                                IdCultura = estudo.IdCultura,
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

                            var dadosEstudo = conexao.Query<Estudo>("SELECT * FROM Estudo Where idEstudo=?", est.IdEstudo);

                            if (dadosEstudo.Count == 0)
                                conexao.Insert(est);
                            else
                                conexao.Update(est);


                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
                return false;

            uri = new Uri($"{App.BackendUrl}/estudo/estudotipoalvo");
            response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();
                tipoAlvoArray = JsonConvert.DeserializeObject<IEnumerable<Estudo_Tipo_Alvo>>(json);

                int total = tipoAlvoArray.Count();
                if (total == 0)
                    return false;

                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                    {
                        foreach (var tipoAlvo in tipoAlvoArray)
                        {
                            var tipoAlvoObj = new Estudo_Tipo_Alvo
                            {
                                IdEstudo_tipo_avaliacao_alvo = tipoAlvo.IdEstudo_tipo_avaliacao_alvo,
                                IdAlvo = tipoAlvo.IdAlvo,
                                idAvaliacao_tipo = tipoAlvo.idAvaliacao_tipo,
                                IdEstudo = tipoAlvo.IdEstudo
                            };

                            conexao.Query<Estudo_Tipo_Alvo>("DELETE FROM Estudo_Tipo_Alvo");

                            conexao.Insert(tipoAlvoObj);        

                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return true;
        }


        public async Task<bool> LocalidadeSync(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/localidade/localidade");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var jsonLoc = await response.Content.ReadAsStringAsync();
                locArray = JsonConvert.DeserializeObject<IEnumerable<Municipio_Localidade>>(jsonLoc);

                int total = locArray.Count();
                if (total > 0)
                {
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
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
                else
                    return false;

            }
            else
                return false;

            return true;
        }

        public async Task<bool> MunicipiosSync(string chave)
        {
            
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/localidade/municipio");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                municipioArray = JsonConvert.DeserializeObject<IEnumerable<Municipio>>(json);

                int total = municipioArray.Count();
                if (total == 0)
                    return false;

                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                    {
                        foreach (var elemento in municipioArray)
                        {
                            var dados = conexao.Query<Municipio>("SELECT * FROM Municipio Where IdMunicipio=?", elemento.IdMunicipio);

                            if (dados.Count == 0)
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
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
                return false;


            return true;
 
        }

        public async Task<bool> BaixarTipoAvaliacao(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/tipoavaliacao");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                tipoAvalArray = JsonConvert.DeserializeObject<IEnumerable<Avaliacao_Tipo>>(json);

                int total = tipoAvalArray.Count();
                if (total == 0)
                    return false;

                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                    {
                        foreach (var tipo in tipoAvalArray)
                        {
                            var avaltipo = new Avaliacao_Tipo
                            {
                                IdAvaliacao_Tipo = tipo.IdAvaliacao_Tipo,
                                Descricao = tipo.Descricao
                            };

                            var dadoTipo = conexao.Query<Cultura>("SELECT * FROM Avaliacao_Tipo Where IdAvaliacao_Tipo=?", tipo.IdAvaliacao_Tipo);

                            if (dadoTipo.Count == 0)
                                conexao.Insert(avaltipo);
                            else
                                conexao.Update(avaltipo);
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> BaixarVariedade(string chave)
        {

            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/variedade");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                variedadeArray = JsonConvert.DeserializeObject<IEnumerable<Cultura_Variedade>>(json);

                int total = variedadeArray.Count();
                if (total == 0)
                    return false;

                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                    {
                        foreach (var variedade in variedadeArray)
                        {
                            var cultvar = new Cultura_Variedade
                            {
                                IdVariedade = variedade.IdVariedade,
                                idCultura = variedade.idCultura,
                                Descricao = variedade.Descricao
                            };

                            var dadoVariedade = conexao.Query<Cultura_Variedade>("SELECT * FROM Cultura_Variedade Where IdVariedade=?", variedade.IdVariedade);

                            if (dadoVariedade.Count == 0)
                                conexao.Insert(cultvar);
                            else
                                conexao.Update(cultvar);
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> BaixarSafra(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/safra");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                safraArray = JsonConvert.DeserializeObject<IEnumerable<Safra>>(json);

                int total = safraArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var safra in safraArray)
                    {
                        var safraObj = new Safra
                        {
                            IdSafra = safra.IdSafra,
                            Descricao = safra.Descricao
                        };

                        var dadoVariedade = conexao.Query<Safra>("SELECT * FROM Safra Where IdSafra=?", safraObj.IdSafra);

                        if (dadoVariedade.Count == 0)
                            conexao.Insert(safraObj);
                        else
                            conexao.Update(safraObj);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;

        }

        public async Task<bool> BaixarUmidade(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/umidadesolo");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                umidadeArray = JsonConvert.DeserializeObject<IEnumerable<Umidade_Solo>>(json);

                int total = umidadeArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var umidade in umidadeArray)
                    {
                        var umidadeObj = new Umidade_Solo
                        {
                            idUmidade_Solo = umidade.idUmidade_Solo,
                            Descricao = umidade.Descricao
                        };

                        var dados = conexao.Query<Umidade_Solo>("SELECT * FROM Umidade_Solo Where idUmidade_Solo=?", umidade.idUmidade_Solo);

                        if (dados.Count == 0)
                            conexao.Insert(umidadeObj);
                        else
                            conexao.Update(umidadeObj);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;

        }


        public async Task<bool> BaixarGleba(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/gleba");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                glebaArray = JsonConvert.DeserializeObject<IEnumerable<Gleba>>(json);

                int total = glebaArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var gleba in glebaArray)
                    {
                        var glebaObj = new Gleba
                        {
                            idGleba = gleba.idGleba,
                            Descricao = gleba.Descricao,
                            Ativo = gleba.Ativo,
                            Metragem = gleba.Metragem
                        };

                        var dados = conexao.Query<Gleba>("SELECT * FROM Gleba Where idGleba=?", gleba.idGleba);

                        if (dados.Count == 0)
                            conexao.Insert(glebaObj);
                        else
                            conexao.Update(glebaObj);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;

        }

        public async Task<bool> BaixarSolo(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/solo");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                soloArray = JsonConvert.DeserializeObject<IEnumerable<Solo>>(json);

                int total = soloArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var solo in soloArray)
                    {
                        var soloObj = new Solo
                        {
                            idSolo = solo.idSolo,
                            Descricao = solo.Descricao
                        };

                        var dados = conexao.Query<Solo>("SELECT * FROM Solo Where idSolo=?", solo.idSolo);

                        if (dados.Count == 0)
                            conexao.Insert(soloObj);
                        else
                            conexao.Update(soloObj);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;

        }

        public async Task<bool> BaixarAlvo(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/alvo");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                alvoArray = JsonConvert.DeserializeObject<IEnumerable<Alvo>>(json);

                int total = alvoArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var alvo in alvoArray)
                    {
                        var alvoObj = new Alvo
                        {
                            IdAlvo = alvo.IdAlvo,
                            Especie = alvo.Especie,
                            Nome_vulgar = alvo.Nome_vulgar
                        };

                        var dados = conexao.Query<Solo>("SELECT * FROM Alvo Where idAlvo=?", alvo.IdAlvo);

                        if (dados.Count == 0)
                            conexao.Insert(alvoObj);
                        else
                            conexao.Update(alvoObj);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;

        }

        public async Task<bool> UsuarioSync(string chave)
        {

            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/users");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                userArray = JsonConvert.DeserializeObject<IEnumerable<Usuario>>(json);

                int total = userArray.Count();
                if (total == 0)
                    return false;

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
                                                        elemento.Senha, elemento.Nome, elemento.IdUsuario);
                            }
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
                return false;

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
