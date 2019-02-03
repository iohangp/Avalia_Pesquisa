using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        IEnumerable<Estudo_Planejamento> planejArray;
        IEnumerable<Cobertura_Solo> coberturaArray;
        IEnumerable<Plantio> plantioArray;
        IEnumerable<Instalacao> instalacaoArray;
        IEnumerable<Avaliacao> avaliacaoArray;

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

            var uri = new Uri($"{App.BackendUrl}/cultura?api_key=1");
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

            var uri = new Uri($"{App.BackendUrl}/estudo?api_key=1");
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

            uri = new Uri($"{App.BackendUrl}/estudo/estudotipoalvo?api_key=1");
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
                        conexao.Query<Estudo_Tipo_Alvo>("DELETE FROM Estudo_Tipo_Alvo");
                        foreach (var tipoAlvo in tipoAlvoArray)
                        {
                            var tipoAlvoObj = new Estudo_Tipo_Alvo
                            {             
                                IdAlvo = tipoAlvo.IdAlvo,
                                idAvaliacao_tipo = tipoAlvo.idAvaliacao_tipo,
                                IdEstudo = tipoAlvo.IdEstudo
                            };          

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

            uri = new Uri($"{App.BackendUrl}/estudo/planejamento?api_key=1");
            response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();
                planejArray = JsonConvert.DeserializeObject<IEnumerable<Estudo_Planejamento>>(json);

                int total = planejArray.Count();
                if (total == 0)
                    return false;

                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                    {
                        conexao.Query<Estudo_Tipo_Alvo>("DELETE FROM Estudo_Planejamento");
                        foreach (var planejamento in planejArray)
                        {
                            var planejObj = new Estudo_Planejamento
                            {
                                idEstudo_planejamento = planejamento.idEstudo_planejamento,
                                idEstudo = planejamento.idEstudo,
                                data = planejamento.data,
                                tipo = 1
                            };

                            conexao.Insert(planejObj);

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

            var uri = new Uri($"{App.BackendUrl}/localidade/localidade?api_key=1");
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

            var uri = new Uri($"{App.BackendUrl}/localidade/municipio?api_key=1");
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

            var uri = new Uri($"{App.BackendUrl}/tipoavaliacao?api_key=1");
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

        public async Task<bool> BaixarCobertura(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/solo/cobertura?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                coberturaArray = JsonConvert.DeserializeObject<IEnumerable<Cobertura_Solo>>(json);

                int total = coberturaArray.Count();
                if (total == 0)
                    return false;

                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                    {
                        foreach (var cobertura in coberturaArray)
                        {
                            var cobertObj = new Cobertura_Solo
                            {
                                idCobertura_Solo = cobertura.idCobertura_Solo,
                                Descricao = cobertura.Descricao
                            };

                            var dadoTipo = conexao.Query<Cultura>("SELECT * FROM Cobertura_Solo Where idCobertura_Solo=?", cobertura.idCobertura_Solo);

                            if (dadoTipo.Count == 0)
                                conexao.Insert(cobertObj);
                            else
                                conexao.Update(cobertObj);
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

            var uri = new Uri($"{App.BackendUrl}/variedade?api_key=1");
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

            var uri = new Uri($"{App.BackendUrl}/safra?api_key=1");
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

            var uri = new Uri($"{App.BackendUrl}/umidadesolo?api_key=1");
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

            var uri = new Uri($"{App.BackendUrl}/gleba?api_key=1");
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

            var uri = new Uri($"{App.BackendUrl}/solo?api_key=1");
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

            var uri = new Uri($"{App.BackendUrl}/alvo?api_key=1");
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

            var uri = new Uri($"{App.BackendUrl}/users?api_key=1");
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


        public async Task<bool> AddPlantio(string chave)
        {
            bool sucesso = true;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    var result = conexao.Query<Plantio>("SELECT * FROM Plantio WHERE Integrado = 0").ToList();

                    if (!CrossConnectivity.Current.IsConnected)
                        return false;

                    foreach (var plant in result)
                    {
                        var plantio = new Plantio
                        {
                            idPlantio = plant.idPlantio,
                            idLocalidade = plant.idLocalidade,
                            idCultura = plant.idCultura,
                            Data_Plantio = plant.Data_Plantio,
                            idVariedade = plant.idVariedade,
                            idSafra = plant.idSafra,
                            Data_Germinacao = plant.Data_Germinacao,
                            idGleba = plant.idGleba,
                            idUmidade_Solo =  plant.idUmidade_Solo,
                            Adubacao_Base = plant.Adubacao_Base,
                            Adubacao_Cobertura = plant.Adubacao_Cobertura,
                            Espacamento = plant.Espacamento,
                            idCulturaAnterior = plant.idCulturaAnterior,
                            idCultura_Cobertura_Solo = plant.idCultura_Cobertura_Solo,
                            idSolo = plant.idSolo,
                            Metragem = plant.Metragem,
                            idUsuario = plant.idUsuario,
                            Observacoes = plant.Observacoes,
                            Populacao = plant.Populacao,
                            Status = plant.Status
                        };

                        var serializedItem = JsonConvert.SerializeObject(plantio);

                        var uri = new Uri($"{App.BackendUrl}/plantio/add?api_key=1");
                        var response = await client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonPost = await response.Content.ReadAsStringAsync();
                            //  dynamic deserializado = JsonConvert.DeserializeObject(jsonPost,typeof(object));
                            dynamic deserializado = JObject.Parse(jsonPost);
                            plantio.IdPlantioWeb = deserializado.idPlantioWeb;
                            plantio.Integrado = 1;
                            conexao.Update(plantio);
                        }
                        else
                            sucesso = false;


                    }

                    if (sucesso)
                        return true;
                    else
                        return false;

                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
           
        }

        public async Task<bool> BaixarPlantio(string chave)
        {

            bool sucesso = true;

            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/plantio?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                plantioArray = JsonConvert.DeserializeObject<IEnumerable<Plantio>>(json);

                int total = plantioArray.Count();
                if (total == 0)
                    return true;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    foreach (var plantio in plantioArray)
                    {
                        var plantioObj = new Plantio
                        {
                            idLocalidade = plantio.idLocalidade,
                            idCultura = plantio.idCultura,
                            Data_Plantio = plantio.Data_Plantio,
                            idVariedade = plantio.idVariedade,
                            idSafra = plantio.idSafra,
                            Data_Germinacao = plantio.Data_Germinacao,
                            idGleba = plantio.idGleba,
                            idUmidade_Solo = plantio.idUmidade_Solo,
                            Adubacao_Base = plantio.Adubacao_Base,
                            Adubacao_Cobertura = plantio.Adubacao_Cobertura,
                            Espacamento = plantio.Espacamento,
                            idCulturaAnterior = plantio.idCulturaAnterior,
                            idCultura_Cobertura_Solo = plantio.idCultura_Cobertura_Solo,
                            idSolo = plantio.idSolo,
                            Metragem = plantio.Metragem,
                            idUsuario = plantio.idUsuario,
                            Observacoes = plantio.Observacoes,
                            Populacao = plantio.Populacao,
                            Status = plantio.Status,
                            Integrado = 2,
                            IdPlantioWeb = plantio.idPlantio
                        };

                        var resultPlant = conexao.Query<Plantio>("SELECT * FROM Plantio WHERE IdPlantioWeb = ?", plantio.idPlantio).ToList();

                        if (resultPlant.Count() > 0)
                            conexao.Update(plantioObj);
                        else
                        {
                            conexao.Insert(plantioObj);
                        }

                    }

                    
                    return true;

                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }

        public async Task<bool> AddInstalacao(string chave)
        {
            bool sucesso = true;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    var result = conexao.Query<Instalacao>("SELECT * FROM Instalacao WHERE idInstalacaoWeb IS NULL").ToList();

                    if (!CrossConnectivity.Current.IsConnected)
                        return false;

                    foreach (var insta in result)
                    {
                        var resultPlant = conexao.Query<Plantio>("SELECT * FROM Plantio WHERE idPlantio = ?", insta.idPlantio).ToList();

                        dynamic instalacao = new ExpandoObject(); ;

                        instalacao.idEstudo = insta.idEstudo;
                        instalacao.idPlantio = insta.idPlantio;
                        instalacao.idPlantioWeb = resultPlant[0].IdPlantioWeb;
                        instalacao.Tamanho_Parcela_Comprimento = insta.Tamanho_Parcela_Comprimento;
                        instalacao.Tamanho_Parcela_Largura = insta.Tamanho_Parcela_Largura;
                        instalacao.Coordenadas1 = insta.Coordenadas1;
                        instalacao.Coordenadas2 = insta.Coordenadas2;
                        instalacao.Altitude = insta.Altitude;
                        instalacao.Data_Instalacao = insta.Data_Instalacao;
                        instalacao.idUsuario = insta.idUsuario;
                        instalacao.Observacoes = insta.Observacoes;
                        instalacao.idStatus = insta.idStatus;

                        var serializedItem = JsonConvert.SerializeObject(instalacao);

                        var uri = new Uri($"{App.BackendUrl}/instalacao/add?api_key=1");
                        var response = await client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

                        Console.WriteLine(response.IsSuccessStatusCode);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonPost = await response.Content.ReadAsStringAsync();
                    
                            dynamic deserializado = JObject.Parse(jsonPost);

                            var instalacaoObj = new Instalacao();
                            instalacaoObj = insta;
                            instalacaoObj.idInstalacaoWeb = deserializado.idInstalacaoWeb;

                            conexao.Update(instalacaoObj);
                        }
                        else
                            sucesso = false;


                    }

                    if (sucesso)
                        return true;
                    else
                        return false;

                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public async Task<bool> BaixarInstalacao(string chave)
        {

            bool sucesso = true;

            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/instalacao?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                instalacaoArray = JsonConvert.DeserializeObject<IEnumerable<Instalacao>>(json);

                int total = instalacaoArray.Count();
                if (total == 0)
                    return true;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    foreach (var instalacao in instalacaoArray)
                    {
                        var instalacaoObj = new Instalacao();
                        instalacaoObj = instalacao;
                        instalacaoObj.idInstalacaoWeb = instalacao.idInstalacao;

                        var resultPlant = conexao.Query<Plantio>("SELECT * FROM Instalacao WHERE idInstalacaoWeb = ?", instalacao.idInstalacao).ToList();

                        if (resultPlant.Count() > 0)
                            conexao.Update(instalacaoObj);
                        else
                        {
                            conexao.Insert(instalacaoObj);
                        }

                    }


                    return true;

                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public async Task<bool> AddAvaliacao(string chave)
        {
            bool sucesso = true;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    var result = conexao.Query<Avaliacao>("SELECT * FROM Avaliacao WHERE idAvaliacaoWeb IS NULL").ToList();

                    if (!CrossConnectivity.Current.IsConnected)
                        return false;

                    foreach (var aval in result)
                    {
                        var resultInsta = conexao.Query<Instalacao>("SELECT * FROM Instalacao WHERE idInstalacao = ?", aval.idInstalacao).ToList();

                        dynamic avaliacao = new ExpandoObject(); ;

                        avaliacao.idInstalacao = aval.idInstalacao;
                        avaliacao.Tratamento = aval.Tratamento;
                        avaliacao.Data = aval.Data;
                        avaliacao.idUsuario = aval.idUsuario;
                        avaliacao.idAvaliacao_Tipo = aval.idAvaliacao_Tipo;
                        avaliacao.Valor = aval.Valor;
                        avaliacao.Observacao = aval.Observacao;
                        avaliacao.Repeticao = aval.Repeticao;
                        avaliacao.idEstudo_Planejamento = aval.idEstudo_Planejamento;
                        avaliacao.idAlvo = aval.idAlvo;
                        avaliacao.idInstalacaoWeb = resultInsta[0].idInstalacaoWeb;
      

                        var serializedItem = JsonConvert.SerializeObject(avaliacao);

                        var uri = new Uri($"{App.BackendUrl}/avaliacao/add?api_key=1");
                        var response = await client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonPost = await response.Content.ReadAsStringAsync();
                            //  dynamic deserializado = JsonConvert.DeserializeObject(jsonPost,typeof(object));
                            dynamic deserializado = JObject.Parse(jsonPost);

                            var avalObject = new Avaliacao();
                            avalObject = aval;
                            avalObject.idAvaliacaoWeb = deserializado.idAvaliacaoWeb;
 
                            conexao.Update(avalObject);
                        }
                        else
                            sucesso = false;


                    }

                    if (sucesso)
                        return true;
                    else
                        return false;

                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public async Task<bool> BaixarAvaliacao(string chave)
        {

            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/avaliacao?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                avaliacaoArray = JsonConvert.DeserializeObject<IEnumerable<Avaliacao>>(json);

                int total = avaliacaoArray.Count();
                if (total == 0)
                    return true;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    foreach (var avaliacao in avaliacaoArray)
                    {
                        var avaliacaoObj = new Avaliacao();
                        avaliacaoObj = avaliacao;
                        avaliacaoObj.idAvaliacaoWeb = avaliacao.IdAvaliacao;

                        var resultAval= conexao.Query<Avaliacao>("SELECT * FROM Avaliacao WHERE idAvaliacaoWeb = ?", avaliacao.IdAvaliacao).ToList();

                        if (resultAval.Count() > 0)
                            conexao.Update(avaliacaoObj);
                        else
                        {
                            conexao.Insert(avaliacaoObj);
                        }

                    }


                    return true;

                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

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
