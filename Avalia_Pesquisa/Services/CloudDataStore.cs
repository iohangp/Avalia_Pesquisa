using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.Util;
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
        IEnumerable<Avaliacao_Imagem> avaliacaoImagemArray;
        IEnumerable<Manutencao> manutencaoArray;
        IEnumerable<Aplicacao> aplicacaoArray;
        IEnumerable<Equipamento> equipamentoArray;
        IEnumerable<Manutencao_Tipo> manutencaoTipoArray;
        IEnumerable<Manutencao_Objetivo> manutencaoObjArray;
        IEnumerable<Aplicacao_Planejamento> aplicPlanArray;
        IEnumerable<Avaliacao_Planejamento> avalPlanArray;
        IEnumerable<Unidade_Medida> unidadeArray;
        IEnumerable<Produto> produtoArray;
        IEnumerable<Estudo_Planejamento_Aplicacao> planAplicArray;
        IEnumerable<Estudo_Planejamento_Avaliacao> planAvalArray;

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
                                Codigo = estudo.Codigo,
                                idCliente = estudo.idCliente,
                                Cliente = estudo.Cliente,
                                idEmpresa = estudo.idEmpresa,
                                Empresa = estudo.Empresa,
                                IdCultura = estudo.IdCultura,
                                //idProduto = estudo.idProduto,
                           //     Produto = estudo.Produto,
                                idClasse = estudo.idClasse,
                                Classe = estudo.Classe,
                                idAlvo = estudo.idAlvo,
                                Alvo = estudo.Alvo,
                                Repeticao = estudo.Repeticao,
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

                /* dynamic deserializado = JArray.Parse(jsonPost);


                 int total = deserializado.Count;
                 if (total == 0)
                     return false; */

                planejArray = JsonConvert.DeserializeObject<IEnumerable<Estudo_Planejamento>>(json);

                int total = planejArray.Count();
                if (total == 0)
                    return false;


                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                    {
                        // conexao.Query<Estudo_Planejamento_Aplicacao>("DELETE FROM Estudo_Planejamento_Aplicacao");
                        //conexao.Query<Estudo_Planejamento_Aplicacao>("DELETE FROM Estudo_Planejamento_Avaliacao");
                        conexao.Query<Estudo_Planejamento>("DELETE FROM Estudo_Planejamento");

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

                        /*  foreach (var planejamento in deserializado)
                          {
                              if (planejamento.tipo == "aplicacao")
                              {
                                  var planejAplic = new Estudo_Planejamento_Aplicacao
                                  {
                                      idEstudo_Planejamento_Aplicacao = planejamento.idPlanejamento,
                                      idEstudo = planejamento.idEstudo,
                                      data = planejamento.data
                                  };

                                  conexao.Insert(planejAplic);

                              }else if (planejamento.tipo == "avaliacao")
                              {
                                  var planejObj = new Estudo_Planejamento_Avaliacao
                                  {
                                      idEstudo_Planejamento_Avaliacao = planejamento.idPlanejamento,
                                      idEstudo = planejamento.idEstudo,
                                      data = planejamento.data
                                  };

                                  conexao.Insert(planejObj);
                              }

                          }*/
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

        public async Task<bool> BaixarAplicPlan(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/estudo/aplicacao?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                aplicPlanArray = JsonConvert.DeserializeObject<IEnumerable<Aplicacao_Planejamento>>(json);

                int total = aplicPlanArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.Query<Aplicacao_Planejamento>("DELETE FROM Aplicacao_Planejamento");
                    foreach (var planejamento in aplicPlanArray)
                    {
                        var aplicPlanjObj = new Aplicacao_Planejamento
                        {
                            idEstudo = planejamento.idEstudo,
                            Dias_Aplicacao = planejamento.Dias_Aplicacao,
                            Num_Aplicacao = planejamento.Num_Aplicacao
                        };

                        conexao.Insert(aplicPlanjObj);

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

        public async Task<bool> BaixarAvalPlan(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/estudo/avaliacao?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                avalPlanArray = JsonConvert.DeserializeObject<IEnumerable<Avaliacao_Planejamento>>(json);

                int total = avalPlanArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    conexao.Query<Avaliacao_Planejamento>("DELETE FROM Avaliacao_Planejamento");
                    foreach (var planejamento in avalPlanArray)
                    {
                        var avalPlanjObj = new Avaliacao_Planejamento
                        {
                            idEstudo = planejamento.idEstudo,
                            Num_Avaliacao = planejamento.Num_Avaliacao,
                            Dias = planejamento.Dias,
                            Apos = planejamento.Apos,
                            idAvaliacao_Tipo = planejamento.idAvaliacao_Tipo,
                            idTipoPlanejamento = planejamento.idTipoPlanejamento,
                            idAlvo = planejamento.idAlvo
                        };

                        conexao.Insert(avalPlanjObj);

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

        public async Task<bool> BaixarEquipamento(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/equipamento?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                equipamentoArray = JsonConvert.DeserializeObject<IEnumerable<Equipamento>>(json);

                int total = equipamentoArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var equip in equipamentoArray)
                    {
                        var equipObj = new Equipamento
                        {
                            IdEquipamento = equip.IdEquipamento,
                            Bicos = equip.Bicos,
                            Descricao = equip.Descricao,
                            Largura = equip.Largura,
                            Volume_Calda = equip.Volume_Calda,
                            Situacao = equip.Situacao
                        };

                        var dadosEquip = conexao.Query<Safra>("SELECT * FROM Equipamento Where IdEquipamento=?", equipObj.IdEquipamento);

                        if (dadosEquip.Count == 0)
                            conexao.Insert(equipObj);
                        else
                            conexao.Update(equipObj);
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

        public async Task<bool> BaixarManutencaoTipo(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/manutencao/tipo/?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                manutencaoTipoArray = JsonConvert.DeserializeObject<IEnumerable<Manutencao_Tipo>>(json);

                int total = manutencaoTipoArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var manTipo in manutencaoTipoArray)
                    {
                        var manTipoObj = new Manutencao_Tipo
                        {
                            idManutencao_Tipo = manTipo.idManutencao_Tipo,
                            Descricao = manTipo.Descricao,
                            Situacao = manTipo.Situacao
                        };

                        var dados = conexao.Query<Manutencao_Tipo>("SELECT * FROM Manutencao_Tipo Where idManutencao_Tipo=?", manTipo.idManutencao_Tipo);

                        if (dados.Count == 0)
                            conexao.Insert(manTipoObj);
                        else
                            conexao.Update(manTipoObj);
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


        public async Task<bool> BaixarManutencaoObj(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/manutencao/objetivo/?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                manutencaoObjArray = JsonConvert.DeserializeObject<IEnumerable<Manutencao_Objetivo>>(json);

                int total = manutencaoObjArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var manObjetivo in manutencaoObjArray)
                    {
                        var manutencaoObj = new Manutencao_Objetivo
                        {
                            idManutencao_Objetivo = manObjetivo.idManutencao_Objetivo,
                            Descricao = manObjetivo.Descricao,
                            Situacao = manObjetivo.Situacao
                        };

                        var dados = conexao.Query<Manutencao_Objetivo>("SELECT * FROM Manutencao_Objetivo Where idManutencao_Objetivo=?", manObjetivo.idManutencao_Objetivo);

                        if (dados.Count == 0)
                            conexao.Insert(manutencaoObj);
                        else
                            conexao.Update(manutencaoObj);
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

        public async Task<bool> BaixarUnidadeMedida(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/manutencao/unidademedida?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                unidadeArray = JsonConvert.DeserializeObject<IEnumerable<Unidade_Medida>>(json);

                int total = unidadeArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var unidade in unidadeArray)
                    {
                        var unidadeObj = new Unidade_Medida
                        {
                            idUnidade_Medida = unidade.idUnidade_Medida,
                            Descricao = unidade.Descricao
                        };

                        var dados = conexao.Query<Unidade_Medida>("SELECT * FROM Unidade_Medida Where idUnidade_Medida=?", unidade.idUnidade_Medida);

                        if (dados.Count == 0)
                            conexao.Insert(unidadeObj);
                        else
                            conexao.Update(unidadeObj);
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

        public async Task<bool> BaixarProdutos(string chave)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/products?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                produtoArray = JsonConvert.DeserializeObject<IEnumerable<Produto>>(json);

                int total = produtoArray.Count();
                if (total == 0)
                    return false;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    foreach (var produto in produtoArray)
                    {
                        var produtoObj = new Produto
                        {
                            idProdutos = produto.idProdutos,
                            Descricao = produto.Descricao,
                            situacao = produto.situacao
                        };

                        var dados = conexao.Query<Unidade_Medida>("SELECT * FROM Produto Where idProdutos=?", produto.idProdutos);

                        if (dados.Count == 0)
                            conexao.Insert(produtoObj);
                        else
                            conexao.Update(produtoObj);
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
                            idUmidade_Solo = plant.idUmidade_Solo,
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

                        var resultPlan = conexao.Query<Estudo_Planejamento_Avaliacao>("SELECT idEstudo_Planejamento_Avaliacao FROM Estudo_Planejamento_Avaliacao WHERE idEstudo_Planejamento_Avaliacao = ?", aval.idEstudo_Planejamento).ToList();
                      //  var resultPlan = conexao.Query<Estudo_Planejamento_Avaliacao>("SELECT * FROM Estudo_Planejamento_Avaliacao").ToList();
                        dynamic avaliacao = new ExpandoObject(); ;

                        avaliacao.idInstalacao = aval.idInstalacao;
                        avaliacao.Tratamento = aval.Tratamento;
                        avaliacao.Data = aval.Data;
                        avaliacao.idUsuario = aval.idUsuario;
                        avaliacao.idAvaliacao_Tipo = aval.idAvaliacao_Tipo;
                        avaliacao.Valor = aval.Valor;
                        avaliacao.Observacao = aval.Observacao;
                        avaliacao.Repeticao = aval.Repeticao;
                        avaliacao.idEstudo_Planejamento = resultPlan[0].idEstudo_Planejamento_Avaliacao;
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
                        /*  var avaliacaoObj = new Avaliacao();
                          avaliacaoObj = avaliacao;
                          avaliacaoObj.idAvaliacaoWeb = avaliacao.IdAvaliacao;
                          */

                        var resultAval = conexao.Query<Avaliacao>("SELECT * FROM Avaliacao WHERE idAvaliacaoWeb = ?", avaliacao.idAvaliacao).ToList();

                        var avaliacaoObj = new Avaliacao
                        {
                           // idAvaliacao = resultAval[0].idAvaliacao,
                            idInstalacao = avaliacao.idInstalacao,
                            Tratamento = avaliacao.Tratamento,
                            Data = avaliacao.Data,
                            idUsuario = avaliacao.idUsuario,
                            idAvaliacao_Tipo = avaliacao.idAvaliacao_Tipo,
                            Valor = avaliacao.Valor,
                            Observacao = avaliacao.Observacao,
                            Repeticao = avaliacao.Repeticao,
                            idAlvo = avaliacao.idAlvo,
                            idEstudo_Planejamento = avaliacao.idEstudo_Planejamento,
                            idAvaliacaoWeb = avaliacao.idAvaliacao
                        };

                        if (resultAval.Count() > 0)
                        {

                          /*  conexao.Query<Avaliacao>("UPDATE Avaliacao " +
                                "  SET idEstudo_Planejamento = ?, " +
                                      " Valor = ? " +
                                " WHERE idAvaliacaoWeb = ?",
                                avaliacao.idEstudo_Planejamento, avaliacao.Valor, avaliacao.idAvaliacaoWeb);
                            */
                            conexao.Update(avaliacaoObj);
                        }
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

        public async Task<bool> AddAplicacao(string chave)
        {
            bool sucesso = true;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {
                    //conexao.Query<Aplicacao>("UPDATE Aplicacao SET idAplicacaoWeb = null WHERE idAplicacao = 10");

                    var result = conexao.Query<Aplicacao>("SELECT * FROM Aplicacao WHERE idAplicacaoWeb IS NULL").ToList();

                    if (!CrossConnectivity.Current.IsConnected)
                        return false;

                    foreach (var aplic in result)
                    {
                        var resultInsta = conexao.Query<Instalacao>("SELECT * FROM Instalacao WHERE idInstalacao = ?", aplic.idInstalacao).ToList();

                        var resultPlan = conexao.Query<Estudo_Planejamento_Aplicacao>("SELECT * FROM Estudo_Planejamento_Aplicacao WHERE idEstudo_Planejamento_Aplicacao = ?", aplic.idEstudo_Planejamento).ToList();

                        dynamic aplicacao = new ExpandoObject(); ;

                        aplicacao.idInstalacao = aplic.idInstalacao;
                        aplicacao.Data_Aplicacao = aplic.Data_Aplicacao;
                        aplicacao.Data_Realizada = aplic.Data_Realizada;
                        aplicacao.Latitude = aplic.Latitude;
                        aplicacao.Longitude = aplic.Longitude;
                        aplicacao.Dosagem = aplic.Dosagem;
                        aplicacao.Umidade_Relativa = aplic.Umidade_Relativa;
                        aplicacao.Temperatura = aplic.Temperatura;
                        aplicacao.Velocidade_Vento = aplic.Velocidade_Vento;
                        aplicacao.Percentual_Nuvens = aplic.Percentual_Nuvens;
                        aplicacao.Chuva_Data = aplic.Chuva_Data;
                        aplicacao.Chuva_Volume = aplic.Chuva_Volume;
                        aplicacao.idEquipamento = aplic.idEquipamento;
                        aplicacao.BBCH = aplic.BBCH;
                        aplicacao.Observacoes = aplic.Observacoes;
                        aplicacao.idUsuario = aplic.idUsuario;
                        aplicacao.idEstudo_Planejamento = resultPlan[0].idEstudo_Planejamento_Aplicacao_Web;
                        aplicacao.idInstalacaoWeb = resultInsta[0].idInstalacaoWeb;


                        var serializedItem = JsonConvert.SerializeObject(aplicacao);

                        var uri = new Uri($"{App.BackendUrl}/aplicacao/add?api_key=1");
                        var response = await client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonPost = await response.Content.ReadAsStringAsync();
                            //  dynamic deserializado = JsonConvert.DeserializeObject(jsonPost,typeof(object));
                            dynamic deserializado = JObject.Parse(jsonPost);

                            var avalObject = new Aplicacao();
                            avalObject = aplic;
                            avalObject.idAplicacaoWeb = deserializado.idAplicacaoWeb;

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

        public async Task<bool> BaixarAplicacao(string chave)
        {

            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/aplicacao?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                aplicacaoArray = JsonConvert.DeserializeObject<IEnumerable<Aplicacao>>(json);

                int total = aplicacaoArray.Count();
                if (total == 0)
                    return true;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    conexao.Query<Aplicacao>("DELETE FROM Aplicacao");

                    foreach (var aplicacao in aplicacaoArray)
                    {
                        var resultPlan = conexao.Query<Estudo_Planejamento_Aplicacao>("SELECT * FROM Estudo_Planejamento_Aplicacao WHERE idEstudo_Planejamento_Aplicacao_Web = ?", aplicacao.idEstudo_Planejamento).ToList();

                        var aplicacaoObj = new Aplicacao();
                        // {
                            aplicacaoObj.idInstalacao = aplicacao.idInstalacao;
                            aplicacaoObj.Data_Aplicacao = aplicacao.Data_Aplicacao;
                            aplicacaoObj.Data_Realizada = aplicacao.Data_Realizada;
                            aplicacaoObj.Latitude = aplicacao.Latitude;
                            aplicacaoObj.Longitude = aplicacao.Longitude;
                            aplicacaoObj.Dosagem = aplicacao.Dosagem;
                            aplicacaoObj.Umidade_Relativa = aplicacao.Umidade_Relativa;
                            aplicacaoObj.Temperatura = aplicacao.Temperatura;
                            aplicacaoObj.Velocidade_Vento = aplicacao.Velocidade_Vento;
                            aplicacaoObj.Percentual_Nuvens = aplicacao.Percentual_Nuvens;
                            aplicacaoObj.Chuva_Data = aplicacao.Chuva_Data;
                            aplicacaoObj.Chuva_Volume = aplicacao.Chuva_Volume;
                            aplicacaoObj.idEquipamento = aplicacao.idEquipamento;
                            aplicacaoObj.BBCH = aplicacao.BBCH;
                            aplicacaoObj.Observacoes = aplicacao.Observacoes;
                            aplicacaoObj.idUsuario = aplicacao.idUsuario;
                            aplicacaoObj.idEstudo_Planejamento = (resultPlan.Count > 0 ? resultPlan[0].idEstudo_Planejamento_Aplicacao : 0);
                            aplicacaoObj.idAplicacaoWeb = aplicacao.IdAplicacao;
                  //  };

                     /*   var resultAplic = conexao.Query<Aplicacao>("SELECT * FROM Aplicacao WHERE idAplicacaoWeb = ?", aplicacao.IdAplicacao).ToList();

                        if (resultAplic.Count() > 0)
                            conexao.Update(aplicacaoObj);
                        else
                        { */
                            conexao.Insert(aplicacaoObj);
                     //   }

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

        public async Task<bool> AddPlanejamentoAplic(string chave)
        {
            bool sucesso = true;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    var result = conexao.Query<Estudo_Planejamento_Aplicacao>("SELECT * FROM Estudo_Planejamento_Aplicacao WHERE idEstudo_Planejamento_Aplicacao_Web IS NULL").ToList();

                    if (!CrossConnectivity.Current.IsConnected)
                        return false;

                    foreach (var aplic in result)
                    {

                        dynamic planejamento = new ExpandoObject(); ;

                        planejamento.idEstudo = aplic.idEstudo;
                        planejamento.Data = aplic.data;
                        planejamento.Num_Aplicacao = aplic.Num_Aplicacao;

                        var serializedItem = JsonConvert.SerializeObject(planejamento);

                        var uri = new Uri($"{App.BackendUrl}/aplicacao/addPlan?api_key=1");
                        var response = await client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonPost = await response.Content.ReadAsStringAsync();
                            //  dynamic deserializado = JsonConvert.DeserializeObject(jsonPost,typeof(object));
                            dynamic deserializado = JObject.Parse(jsonPost);

                            var avalObject = new Estudo_Planejamento_Aplicacao();
                            avalObject = aplic;
                            avalObject.idEstudo_Planejamento_Aplicacao_Web = deserializado.idPlanejamentoWeb;

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

        public async Task<bool> BaixarPlanejamentoAplic(string chave)
        {

            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/aplicacao/planejamento?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                planAplicArray = JsonConvert.DeserializeObject<IEnumerable<Estudo_Planejamento_Aplicacao>>(json);

                int total = planAplicArray.Count();
                if (total == 0)
                    return true;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {              

                    conexao.Query<Estudo_Planejamento_Aplicacao>("DELETE FROM Estudo_Planejamento_Aplicacao");

                    foreach (var aplicacao in planAplicArray)
                    {
                        var aplicacaoObj = new Estudo_Planejamento_Aplicacao
                        {
                            idEstudo = aplicacao.idEstudo,
                            Num_Aplicacao = aplicacao.Num_Aplicacao,
                            data = aplicacao.data,
                            idEstudo_Planejamento_Aplicacao_Web = aplicacao.idEstudo_Planejamento_Aplicacao
                        };
                   //     aplicacaoObj = aplicacao;
                    //    aplicacaoObj.idEstudo_Planejamento_Aplicacao_Web = aplicacao.idEstudo_Planejamento_Aplicacao;

                    //       var resultAplic = conexao.Query<Aplicacao>("SELECT * FROM Estudo_Planejamento_Aplicacao WHERE idEstudo_Planejamento_Aplicacao_Web = ?", aplicacao.idEstudo_Planejamento_Aplicacao).ToList();

      
                            conexao.Insert(aplicacaoObj);


                        /*  conexao.Query<Estudo_Planejamento_Aplicacao>("INSERT INTO Estudo_Planejamento_Aplicacao (" +
                                                                       "idEstudo_Planejamento_Aplicacao," +
                                                                       "idEstudo, " +
                                                                       "Num_Aplicacao, " +
                                                                       "data," +
                                                                       "idEstudo_Planejamento_Aplicacao_Web) " +
                                                                       "VALUES (?,?,?,?,?)",
                                                                       aplicacao.idEstudo_Planejamento_Aplicacao,
                                                                       aplicacao.idEstudo,
                                                                       aplicacao.Num_Aplicacao,
                                                                       aplicacao.data,
                                                                       aplicacao.idEstudo_Planejamento_Aplicacao);
                        */
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

      public async Task<bool> AddPlanejamentoAval(string chave)
      {
          bool sucesso = true;
          try
          {
              using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
              {

                  var result = conexao.Query<Estudo_Planejamento_Avaliacao>("SELECT * FROM Estudo_Planejamento_Avaliacao WHERE Integrado = 0").ToList();

                  if (!CrossConnectivity.Current.IsConnected)
                      return false;

                  foreach (var aval in result)
                  {

                      dynamic planejamento = new ExpandoObject(); ;

                      planejamento.idEstudo_Planejamento_Avaliacao = aval.idEstudo_Planejamento_Avaliacao;
                      planejamento.idEstudo = aval.idEstudo;
                      planejamento.Data = aval.data;
                      planejamento.Num_Avaliacao = aval.Num_Avaliacao;
                      planejamento.idAvaliacao_Tipo = aval.idAvaliacao_Tipo;
                      planejamento.idAlvo = aval.idAlvo;

                      var serializedItem = JsonConvert.SerializeObject(planejamento);

                      var uri = new Uri($"{App.BackendUrl}/avaliacao/addPlan?api_key=1");
                      var response = await client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

                      if (response.IsSuccessStatusCode)
                      {
                          var jsonPost = await response.Content.ReadAsStringAsync();
                          //  dynamic deserializado = JsonConvert.DeserializeObject(jsonPost,typeof(object));
                        //  dynamic deserializado = JObject.Parse(jsonPost);

                          var avalObject = new Estudo_Planejamento_Avaliacao();
                          avalObject = aval;
                          avalObject.Integrado = 1;

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

      public async Task<bool> BaixarPlanejamentoAval(string chave)
      {

          if (!CrossConnectivity.Current.IsConnected)
              return false;

          var uri = new Uri($"{App.BackendUrl}/avaliacao/planejamento?api_key=1");
          var response = await client.GetAsync(uri);
          if (response.IsSuccessStatusCode)
          {
              var json = await response.Content.ReadAsStringAsync();
              planAvalArray = JsonConvert.DeserializeObject<IEnumerable<Estudo_Planejamento_Avaliacao>>(json);

              int total = planAvalArray.Count();
              if (total == 0)
                  return true;
          }

          try
          {
              using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
              {

                  conexao.Query<Estudo_Planejamento_Avaliacao>("DELETE FROM Estudo_Planejamento_Avaliacao");

                  foreach (var avaliacao in planAvalArray)
                  {
                      var avaliacaoObj = new Estudo_Planejamento_Avaliacao();
                      avaliacaoObj = avaliacao;
                      avaliacaoObj.idEstudo_Planejamento_Avaliacao_Web = avaliacao.idEstudo_Planejamento_Avaliacao;

                      // var resultAplic = conexao.Query<Estudo_Planejamento_Avaliacao>("SELECT * FROM Estudo_Planejamento_Avaliacao WHERE idEstudo_Planejamento_Avaliacao_Web = ?", avaliacao.idEstudo_Planejamento_Avaliacao).ToList();

                      /* if (resultAplic.Count() > 0)
                           conexao.Update(avaliacaoObj);
                       else
                       {
                           conexao.Insert(avaliacaoObj);
                       }*/

                        conexao.Query<Estudo_Planejamento_Avaliacao>("INSERT INTO Estudo_Planejamento_Avaliacao (" +
                                                                        "idEstudo_Planejamento_Avaliacao," +
                                                                        "idEstudo, " +
                                                                        "Num_Avaliacao, " +
                                                                        "data," +
                                                                        "idAvaliacao_Tipo, " +
                                                                        "idAlvo," +
                                                                        "Integrado) " +
                                                                        "VALUES (?,?,?,?,?,?,?)",
                                                                        avaliacao.idEstudo_Planejamento_Avaliacao,
                                                                        avaliacao.idEstudo,
                                                                        avaliacao.Num_Avaliacao,
                                                                        avaliacao.data,
                                                                        avaliacao.idAvaliacao_Tipo,
                                                                        avaliacao.idAlvo,
                                                                        1);

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

        public async Task<bool> AddManutencao(string chave)
        {
            bool sucesso = true;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    var result = conexao.Query<Manutencao>("SELECT * FROM Manutencao WHERE idManutencaoWeb IS NULL").ToList();

                    if (!CrossConnectivity.Current.IsConnected)
                        return false;

                    foreach (var manu in result)
                    {
                        var resultInsta = conexao.Query<Instalacao>("SELECT * FROM Instalacao WHERE idInstalacao = ?", manu.idInstalacao).ToList();

                        dynamic manutencao = new ExpandoObject(); ;

                        manutencao.idInstalacao = manu.idInstalacao;
                        manutencao.Data = manu.Data;
                        manutencao.idProduto = manu.idProduto;
                        manutencao.Latitude = manu.Latitude;
                        manutencao.Longitude = manu.Longitude;
                        manutencao.Dose = manu.Dose;
                        manutencao.Umidade_Relativa = manu.Umidade_Relativa;
                        manutencao.Temperatura = manu.Temperatura;
                        manutencao.Velocidade_Vento = manu.Velocidade_Vento;
                        manutencao.Percentual_Nuvens = manu.Percentual_Nuvens;
                        manutencao.idUnidade_Medida = manu.idUnidade_Medida;
                        manutencao.idManutencao_Tipo = manu.idManutencao_Tipo;
                        manutencao.idManutencao_Objetivo = manu.idManutencao_Objetivo;
                        manutencao.Hora_Inicio_Fim = manu.Hora_Inicio_Fim;
                        manutencao.Observacoes = manu.Observacoes;
                        manutencao.idUsuario = manu.idUsuario;
                        manutencao.idInstalacaoWeb = resultInsta[0].idInstalacaoWeb;


                        var serializedItem = JsonConvert.SerializeObject(manutencao);

                        var uri = new Uri($"{App.BackendUrl}/manutencao/add?api_key=1");
                        var response = await client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonPost = await response.Content.ReadAsStringAsync();
                            //  dynamic deserializado = JsonConvert.DeserializeObject(jsonPost,typeof(object));
                            dynamic deserializado = JObject.Parse(jsonPost);

                            var manuObject = new Manutencao();
                            manuObject = manu;
                            manuObject.idManutencaoWeb = deserializado.idManutencaoWeb;

                            conexao.Update(manuObject);
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

        public async Task<bool> AddAvaliacaoImagem(string chave)
        {
            bool sucesso = true;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    var result = conexao.Query<Avaliacao_Imagem>("SELECT * FROM Avaliacao_Imagem WHERE (idAvaliacao_ImagemWeb IS NULL OR idAvaliacao_ImagemWeb = 0)").ToList();

                    if (!CrossConnectivity.Current.IsConnected)
                        return false;

                    foreach (var Aval_Img in result)
                    {
                        var resultInsta = conexao.Query<Avaliacao>("SELECT * FROM Avaliacao WHERE idAvaliacao = ?", Aval_Img.idAvaliacao).ToList();

                        dynamic avaliacao_imagem = new ExpandoObject();

                        avaliacao_imagem.idAvaliacao = Aval_Img.idAvaliacao;
                        avaliacao_imagem.Data = Aval_Img.Data;
                        avaliacao_imagem.Imagem = Aval_Img.Imagem;
                        avaliacao_imagem.Tratamento = Aval_Img.Tratamento;
                        avaliacao_imagem.Repeticao = Aval_Img.Repeticao;
                        avaliacao_imagem.idUsuario = Aval_Img.idUsuario;
                        avaliacao_imagem.idAvaliacaoWeb = resultInsta[0].idAvaliacaoWeb;


                        var serializedItem = JsonConvert.SerializeObject(avaliacao_imagem);

                        var uri = new Uri($"{App.BackendUrl}/avaliacao/addFoto?api_key=1");
                        var response = await client.PostAsync(uri, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonPost = await response.Content.ReadAsStringAsync();
                            //  dynamic deserializado = JsonConvert.DeserializeObject(jsonPost,typeof(object));
                            dynamic deserializado = JObject.Parse(jsonPost);

                            var avalimagemObject = new Avaliacao_Imagem();
                            avalimagemObject = Aval_Img;
                            avalimagemObject.idAvaliacao_ImagemWeb = deserializado.idAvaliacaoImagemWeb;

                            conexao.Update(avalimagemObject);
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

        public async Task<bool> BaixarManutencao(string chave)
        {

            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var uri = new Uri($"{App.BackendUrl}/manutencao?api_key=1");
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                manutencaoArray = JsonConvert.DeserializeObject<IEnumerable<Manutencao>>(json);

                int total = manutencaoArray.Count();
                if (total == 0)
                    return true;
            }

            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "AvaliaPesquisa.db")))
                {

                    foreach (var manutencao in manutencaoArray)
                    {
                        var manutencaoObj = new Manutencao();
                        manutencaoObj = manutencao;
                        manutencaoObj.idManutencaoWeb = manutencao.idManutencao;

                        var resultManu = conexao.Query<Manutencao>("SELECT * FROM Manutencao WHERE idManutencaoWeb = ?", manutencao.idManutencao).ToList();

                        if (resultManu.Count() > 0)
                            conexao.Update(manutencaoObj);
                        else
                        {
                            conexao.Insert(manutencaoObj);
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

