﻿
using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Avalia_Pesquisa.Droid.Helpers;
using Plugin.Media;
using System.Data;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Avaliar Estudo")]
    public class AvaliacaoActivity : BaseActivity
    {
        Spinner spnTipo, spnAlvo;
        ArrayAdapter adapter;
        ArrayList tipos, idTipos, alvos, idAlvos;
        EditText edNumEstudo, etRepeticao1, etRepeticao2, etRepeticao3, etRepeticao4, etRepeticao5;
        int totalRepeticoes = 1, idEstudo, idPlanejamento, idInstalacao, Tratamento, Num_Avaliacao;
        string idTipoAvaliacao, idAlvoSelect;
        TableRow rowRepeticao1, rowRepeticao2, rowRepeticao3, rowRepeticao4, rowRepeticao5,
                 rowAlvo, rowTipoAval, rowPlanejamento, rowTratamento;
        Button buttonSalvar;
        TextView textData, textTratamento, textNumAval;
        ImageButton buttonCamera1, buttonCamera2, buttonCamera3, buttonCamera4;
        byte[] byteArray;
       
        DataTable dt = new DataTable();

        protected override int LayoutResource => Resource.Layout.Avaliacao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //  SetContentView(Resource.Layout.Avaliacao);
            // Create your application here
            spnTipo = FindViewById<Spinner>(Resource.Id.spnTipoAvaliacao);
            spnAlvo = FindViewById<Spinner>(Resource.Id.spnAlvo);
            edNumEstudo = FindViewById<EditText>(Resource.Id.EDNumEstudo);
            buttonSalvar = FindViewById<Button>(Resource.Id.BTSalvarAvaliacao);
            textData = FindViewById<TextView>(Resource.Id.tvDataPlan);
            textTratamento = FindViewById<TextView>(Resource.Id.tvTratamento);
            textNumAval = FindViewById<TextView>(Resource.Id.tvNumAval);
            Button buttonValida = FindViewById<Button>(Resource.Id.BTValidar);
            Button buttonScan = FindViewById<Button>(Resource.Id.BTScannerAvalia);

            rowRepeticao1 = FindViewById<TableRow>(Resource.Id.trRepeticao1);
            rowRepeticao2 = FindViewById<TableRow>(Resource.Id.trRepeticao2);
            rowRepeticao3 = FindViewById<TableRow>(Resource.Id.trRepeticao3);
            rowRepeticao4 = FindViewById<TableRow>(Resource.Id.trRepeticao4);
            rowRepeticao5 = FindViewById<TableRow>(Resource.Id.trRepeticao5);

            rowAlvo = FindViewById<TableRow>(Resource.Id.trAlvo);
            rowTipoAval = FindViewById<TableRow>(Resource.Id.trTipoAvaliacao);
          //  rowPlanejamento = FindViewById<TableRow>(Resource.Id.trDataPlanejada);
         //   rowTratamento = FindViewById<TableRow>(Resource.Id.trTratamento);

            etRepeticao1 = FindViewById<EditText>(Resource.Id.etRepeticao1);
            etRepeticao2 = FindViewById<EditText>(Resource.Id.etRepeticao2);
            etRepeticao3 = FindViewById<EditText>(Resource.Id.etRepeticao3);
            etRepeticao4 = FindViewById<EditText>(Resource.Id.etRepeticao4);
            etRepeticao5 = FindViewById<EditText>(Resource.Id.etRepeticao5);

            buttonCamera1 = FindViewById<ImageButton>(Resource.Id.ibCamera1);
            buttonCamera2 = FindViewById<ImageButton>(Resource.Id.ibCamera2);
            buttonCamera3 = FindViewById<ImageButton>(Resource.Id.ibCamera3);
            buttonCamera4 = FindViewById<ImageButton>(Resource.Id.ibCamera4);

            buttonCamera1.Click += Camera1_OnClick;
            buttonCamera2.Click += Camera2_OnClick;
            buttonCamera3.Click += Camera3_OnClick;
            buttonCamera4.Click += Camera4_OnClick;

            buttonScan.Click += BTScanner_Click;
            buttonSalvar.Click += BTSalvar_Click;

            buttonValida.Click += (sender, e) =>
            {
                ValidarEstudo(edNumEstudo.Text);
            };

            spnTipo.ItemSelected += SpnTipo_ItemSelected;
            spnAlvo.ItemSelected += SpnAlvo_ItemSelected;

            dt.Columns.Add("imagem", typeof(string));
            dt.Columns.Add("repeticao", typeof(int));
            dt.Columns.Add("tratamento", typeof(int));
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            edNumEstudo = FindViewById<EditText>(Resource.Id.EDNumEstudo);

            if (requestCode == 1)
            {
                if (resultCode == Result.Ok)
                {
                    if (data.GetStringExtra("qrcode") != null)
                    {
                        //edNumEstudo.Text = data.GetStringExtra("qrcode");
                        //ValidarEstudo(edNumEstudo.Text);
                        string codigo = data.GetStringExtra("qrcode");
                        ValidarEstudo(codigo);
                    }
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        private async void Camera1_OnClick(object sender, EventArgs e)
        {

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                try
                {
                    var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

                    using (var memoryStream = new MemoryStream())
                    {
                        photo.GetStream().CopyTo(memoryStream);
                        photo.Dispose();
                        byteArray = memoryStream.ToArray();
                        //myAL.Add(byteArray.ToString());
                        //myAL.Add("1");
                        //myAL.Add(textTratamento.Text);



                        dt.Rows.Add(new object[] { byteArray.ToString(), 1, textTratamento.Text });

                    }

                }

               catch { Toast.MakeText(this, "Imagem não capturada", ToastLength.Long).Show(); }
            }
            else
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                AlertDialog alerta = builder.Create();

                alerta.SetTitle("ERRO!");
                alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                alerta.SetMessage("Erro ao ativar a camera!");
                alerta.SetButton("OK", (s, ev) =>
                {
                    alerta.Dismiss();
                });
                alerta.Show();

            }
        }

        private async void Camera2_OnClick(object sender, EventArgs e)
        {

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                try
                {
                    var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

                    using (var memoryStream = new MemoryStream())
                    {
                        photo.GetStream().CopyTo(memoryStream);
                        photo.Dispose();
                        byteArray = memoryStream.ToArray();


                        dt.Rows.Add(new object[] { byteArray, 2, textTratamento.Text });
                    }

                }

                catch { Toast.MakeText(this, "Imagem não capturada", ToastLength.Long).Show(); }
            }
            else
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                AlertDialog alerta = builder.Create();

                alerta.SetTitle("ERRO!");
                alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                alerta.SetMessage("Erro ao ativar a camera!");
                alerta.SetButton("OK", (s, ev) =>
                {
                    alerta.Dismiss();
                });
                alerta.Show();

            }
        }

        private async void Camera3_OnClick(object sender, EventArgs e)
        {

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                try
                {
                    var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
                    using (var memoryStream = new MemoryStream())
                    {
                        photo.GetStream().CopyTo(memoryStream);
                        photo.Dispose();
                        byteArray = memoryStream.ToArray();


                        dt.Rows.Add(new object[] { byteArray, 3, textTratamento.Text });
                    }
                }

                catch { Toast.MakeText(this, "Imagem não capturada", ToastLength.Long).Show(); }
            }
            else
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                AlertDialog alerta = builder.Create();

                alerta.SetTitle("ERRO!");
                alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                alerta.SetMessage("Erro ao ativar a camera!");
                alerta.SetButton("OK", (s, ev) =>
                {
                    alerta.Dismiss();
                });
                alerta.Show();
            }
        }

        private async void Camera4_OnClick(object sender, EventArgs e)
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                try
                {
                    var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

                    using (var memoryStream = new MemoryStream())
                    {
                        photo.GetStream().CopyTo(memoryStream);
                        photo.Dispose();
                        byteArray = memoryStream.ToArray();


                        dt.Rows.Add(new object[] { byteArray, 4, textTratamento.Text });
                    }
                }
                catch { Toast.MakeText(this, "Imagem não capturada", ToastLength.Long).Show(); }
            }
            else
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                AlertDialog alerta = builder.Create();

                alerta.SetTitle("ERRO!");
                alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                alerta.SetMessage("Erro ao ativar a camera!");
                alerta.SetButton("OK", (s, ev) =>
                {
                    alerta.Dismiss();
                });
                alerta.Show();

            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.OnBackPressed();
                    return true;

            }
            return base.OnOptionsItemSelected(item);
        }

        //Botao Voltar do celular
        public override void OnBackPressed()
        {
            Finish();
        }

        private void SpnTipo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idTipoAvaliacao = idTipos[e.Position].ToString();

            if (int.Parse(idTipoAvaliacao) > 0)
                GetAlvos(int.Parse(idTipoAvaliacao), idEstudo);
        }

        private void SpnAlvo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idAlvoSelect = idAlvos[e.Position].ToString();

            if (int.Parse(idAlvoSelect) > 0)
            {
                AvaliacaoService ava = new AvaliacaoService();
                var result = ava.GetPlanejamentoAlvo(idEstudo, Tratamento, int.Parse(idTipoAvaliacao), int.Parse(idAlvoSelect));

                idPlanejamento = result[0].idEstudo_Planejamento_Avaliacao;
                
                dynamic planEstudo = ava.GetPlanejamentoEstudo(idEstudo, int.Parse(idAlvoSelect), int.Parse(idTipoAvaliacao), result[0].Num_Avaliacao);

                textNumAval.Text = planEstudo.numAval;
                textData.Text = planEstudo.dataAval;

            }
        }

        protected internal void BTScanner_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(QrCodeActivity));
            StartActivityForResult(intent, 1);
        }

        protected internal void BTSalvar_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alerta = new AlertDialog.Builder(this);

            if (ValidarData(idEstudo, Tratamento))
            {
                alerta.SetTitle("Atenção!");
                alerta.SetIcon(Android.Resource.Drawable.IcInputAdd);
                //define a mensagem
                alerta.SetMessage("Você está avaliando fora da data planejada. Deseja prosseguir?");
                //define o botão positivo
                alerta.SetPositiveButton("Sim", (senderAlert, args) =>
                {
                    SalvarAvaliacao();
                });
                alerta.SetNegativeButton("Não", (senderAlert, args) =>
                {

                });
                //cria o alerta e exibe
                Dialog dialog = alerta.Create();
                dialog.Show();
            }
            else
            {
                SalvarAvaliacao();
            } 
        }

        private void SalvarAvaliacao()
        {
            bool sucesso = true;
            int cont = 1;
            etRepeticao1 = FindViewById<EditText>(Resource.Id.etRepeticao1);
            etRepeticao2 = FindViewById<EditText>(Resource.Id.etRepeticao2);
            etRepeticao3 = FindViewById<EditText>(Resource.Id.etRepeticao3);
            etRepeticao4 = FindViewById<EditText>(Resource.Id.etRepeticao4);
            etRepeticao5 = FindViewById<EditText>(Resource.Id.etRepeticao5);


            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();

            while (totalRepeticoes >= cont)
            {
                if (cont == 1 && etRepeticao1.Text == "")
                    sucesso = false;
                if (cont == 2 && etRepeticao2.Text == "")
                    sucesso = false;
                if (cont == 3 && etRepeticao3.Text == "")
                    sucesso = false;
                if (cont == 4 && etRepeticao4.Text == "")
                    sucesso = false;
                if (cont == 5 && etRepeticao5.Text == "")
                    sucesso = false;
                cont++;
            }

            if (sucesso)
            {
                AvaliacaoService avalService = new AvaliacaoService();
                if (etRepeticao1.Text != "")
                {
                    var aval = new Avaliacao
                    {
                        idInstalacao = idInstalacao,
                        Tratamento = Tratamento,
                        Repeticao = 1,
                        Valor = decimal.Parse(etRepeticao1.Text.Replace(".", ",")),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Data = DateTime.Now,
                        idAvaliacao_Tipo = int.Parse(idTipoAvaliacao),
                        idAlvo = int.Parse(idAlvoSelect),
                        idEstudo_Planejamento = idPlanejamento,
                        Integrado = 0
                    };
                    if (!avalService.SalvarAvaliacao(aval))
                    {
                        sucesso = false;

                    }
                    else SalvarImagem(1);

                }
                if (etRepeticao2.Text != "")
                {
                    var aval = new Avaliacao
                    {
                        idInstalacao = idInstalacao,
                        Tratamento = Tratamento,
                        Repeticao = 2,
                        Valor = decimal.Parse(etRepeticao2.Text.Replace(".", ",")),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Data = DateTime.Now,
                        idAvaliacao_Tipo = int.Parse(idTipoAvaliacao),
                        idAlvo = int.Parse(idAlvoSelect),
                        idEstudo_Planejamento = idPlanejamento,
                        Integrado = 0
                    };
                    if (!avalService.SalvarAvaliacao(aval))
                    {
                        sucesso = false;

                    }
                    else SalvarImagem(2);

                }
                if (etRepeticao3.Text != "")
                {
                    var aval = new Avaliacao
                    {
                        idInstalacao = idInstalacao,
                        Tratamento = Tratamento,
                        Repeticao = 3,
                        Valor = decimal.Parse(etRepeticao3.Text.Replace(".", ",")),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Data = DateTime.Now,
                        idAvaliacao_Tipo = int.Parse(idTipoAvaliacao),
                        idAlvo = int.Parse(idAlvoSelect),
                        idEstudo_Planejamento = idPlanejamento,
                        Integrado = 0
                    };
                    if (!avalService.SalvarAvaliacao(aval))
                    {
                        sucesso = false;

                    }
                    else SalvarImagem(3);

                }
                if (etRepeticao4.Text != "")
                {
                    var aval = new Avaliacao
                    {
                        idInstalacao = idInstalacao,
                        Tratamento = Tratamento,
                        Repeticao = 4,
                        Valor = decimal.Parse(etRepeticao4.Text.Replace(".", ",")),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Data = DateTime.Now,
                        idAvaliacao_Tipo = int.Parse(idTipoAvaliacao),
                        idAlvo = int.Parse(idAlvoSelect),
                        idEstudo_Planejamento = idPlanejamento,
                        Integrado = 0
                    };
                    if (!avalService.SalvarAvaliacao(aval))
                        sucesso = false;
                    else SalvarImagem(4);

                }
                if (etRepeticao5.Text != "")
                {
                    var aval = new Avaliacao
                    {
                        idInstalacao = idInstalacao,
                        Tratamento = Tratamento,
                        Repeticao = 5,
                        Valor = decimal.Parse(etRepeticao5.Text.Replace(".", ",")),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Data = DateTime.Now,
                        idAvaliacao_Tipo = int.Parse(idTipoAvaliacao),
                        idAlvo = int.Parse(idAlvoSelect),
                        idEstudo_Planejamento = idPlanejamento,
                        Integrado = 0
                    };
                    if (!avalService.SalvarAvaliacao(aval))
                    {
                        sucesso = false;

                    }
                    else SalvarImagem(5);
                }

                if (sucesso)
                {

                    //etRepeticao1.Text = etRepeticao2.Text = etRepeticao3.Text = etRepeticao4.Text = etRepeticao5.Text = edNumEstudo.Text = "";
                    EscondeCampos();
                    alerta.SetTitle("Sucesso!");
                    alerta.SetIcon(Android.Resource.Drawable.IcDialogInfo);
                    alerta.SetMessage("Avaliação Salva com Sucesso!");
                    alerta.SetButton("OK", (s, ev) =>
                    {
                        AvaliacaoService aval = new AvaliacaoService();
                        var plan = aval.GetDataAvaliacao(idEstudo, Tratamento);

                        if (plan.Count > 0)
                        {
                          //  idPlanejamento = plan[0].idEstudo_Planejamento_Avaliacao;
                            textData.Text = "";
                            textNumAval.Text = "";
 

                            GetAvaliacaoTipo(idEstudo, Tratamento);

                            alvos = new ArrayList();
                            idAlvos = new ArrayList();
                            alvos.Add("Selecione");
                            idAlvos.Add(0);
                        }
                        else
                        {
                            EscondeCampos();
                            Toast.MakeText(this, "Todas as avaliações para este estudo foram concluídas!", ToastLength.Long).Show();
                        }

                        alerta.Dismiss();
                    });
                    alerta.Show();
                }
                else
                {
                    alerta.SetTitle("ERRO!");
                    alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alerta.SetMessage("Erro ao salvar a Avaliação!");
                    alerta.SetButton("OK", (s, ev) =>
                    {
                        alerta.Dismiss();
                    });
                    alerta.Show();
                }
            }
            else
            {
                alerta.SetTitle("ERRO!");
                alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                alerta.SetMessage("Informe um valor para todas as repetições");
                alerta.SetButton("OK", (s, ev) =>
                {
                    alerta.Dismiss();
                });
                alerta.Show();
            }

        }

        private void ValidarEstudo(string protocolo)
        {
            int numRepeticao = 1;

            string[] ids = new string[2];


            if (!string.IsNullOrEmpty(protocolo))
            {

                if (protocolo.IndexOf('|') != -1)
                    ids = protocolo.Split('|');
                else
                    ids[0] = protocolo;


                ConsultaEstudoService ces = new ConsultaEstudoService();
                var estudo = ces.GetEstudo(ids[0]);

                idInstalacao = default(int);
                if (estudo.Count > 0)
                {

                    idEstudo = estudo[0].IdEstudo;
                    totalRepeticoes = estudo[0].Repeticao;
                    idInstalacao = estudo[0].idInstalacao;
                    Tratamento = int.Parse(ids[1]);
                    textTratamento.Text = Tratamento.ToString();

                    edNumEstudo.Text = estudo[0].Codigo;

                    AvaliacaoService aval = new AvaliacaoService();
                    var plan = aval.GetDataAvaliacao(idEstudo,Tratamento);
              
                    if (plan.Count > 0) {

                        //  idPlanejamento = plan[0].idEstudo_Planejamento_Avaliacao;
                        textData.Text = "";
                        textNumAval.Text = "";
                      //  Num_Avaliacao = plan[0].Num_Avaliacao;

                        GetAvaliacaoTipo(idEstudo, Tratamento);
                        rowTipoAval.Visibility = ViewStates.Visible;
                        rowAlvo.Visibility = ViewStates.Visible;
                      //  rowTratamento.Visibility = ViewStates.Visible;
                     //   rowPlanejamento.Visibility = ViewStates.Visible;

                        while (estudo[0].Repeticao >= numRepeticao)
                        {
                            if (numRepeticao == 1)
                                rowRepeticao1.Visibility = ViewStates.Visible;
                            else if (numRepeticao == 2)
                                rowRepeticao2.Visibility = ViewStates.Visible;
                            else if (numRepeticao == 3)
                                rowRepeticao3.Visibility = ViewStates.Visible;
                            else if (numRepeticao == 4)
                                rowRepeticao4.Visibility = ViewStates.Visible;
                            else if (numRepeticao == 5)
                                rowRepeticao5.Visibility = ViewStates.Visible;

                            numRepeticao++;
                        }
                        buttonSalvar.Visibility = ViewStates.Visible;
                    }
                    else
                    {

                        EscondeCampos();
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        AlertDialog alerta = builder.Create();

                        alerta.SetTitle("Atenção!");
                        alerta.SetIcon(Android.Resource.Drawable.IcDelete);
                        alerta.SetMessage("Todas as avaliações para este estudo já foram realizadas");
                        alerta.SetButton("OK", (s, ev) =>
                        {
                            alerta.Dismiss();
                        });
                        alerta.Show();

                    } 
                }
                else
                {
                    EscondeCampos();
                    Toast.MakeText(this, "Nenhum estudo encontrado", ToastLength.Long).Show();
                }
            }
            else
            {
                EscondeCampos();
                Toast.MakeText(this, "Informe o código ou utilize a função Scanner", ToastLength.Long).Show();
            }


        }

        private void EscondeCampos()
        {
            etRepeticao1.Text = etRepeticao2.Text = etRepeticao3.Text = etRepeticao4.Text = etRepeticao5.Text = edNumEstudo.Text = "";
            rowRepeticao1.Visibility = ViewStates.Invisible;
            rowRepeticao2.Visibility = ViewStates.Invisible;
            rowRepeticao3.Visibility = ViewStates.Invisible;
            rowRepeticao4.Visibility = ViewStates.Invisible;
            rowRepeticao5.Visibility = ViewStates.Invisible;

            rowTipoAval.Visibility = ViewStates.Invisible;
            rowAlvo.Visibility = ViewStates.Invisible;
       //     rowPlanejamento.Visibility = ViewStates.Invisible;
       //     rowTratamento.Visibility = ViewStates.Invisible;

            buttonSalvar.Visibility = ViewStates.Invisible;
        }

        private bool ValidarData(int idEstudo, int Tratamento)
        {
            AvaliacaoService aval = new AvaliacaoService();
            var plan = aval.GetDataAvaliacao(idEstudo, Tratamento);

            if (plan[0].data > DateTime.Now)
                return true;
            else
                return false;

        }

        private void GetAvaliacaoTipo(int idEstudo, int Tratamento)
        {
            tipos = new ArrayList();
            idTipos = new ArrayList();
            TipoAvaliacaoService tas = new TipoAvaliacaoService();

            tipos.Add("Selecione");
            idTipos.Add(0);

            var result = tas.GetAvaliacaoTipo(idEstudo, Tratamento);

            foreach (var res in result)
            {
                tipos.Add(res.Descricao);
                idTipos.Add(res.IdAvaliacao_Tipo);
            }

            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, tipos);
            spnTipo.Adapter = adapter;

            alvos = new ArrayList();
            idAlvos = new ArrayList();
            alvos.Add("Selecione");
            idAlvos.Add(0);
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, alvos);
            spnAlvo.Adapter = adapter;

        }

        public void SalvarImagem(int repeticao)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();
            AvaliacaoService avaliacaoService = new AvaliacaoService();


            int IdAvaliacao = avaliacaoService.GetUltimaAvaliacao()[0].idAvaliacao;
            //idAvaliacao = int.Parse(avaliacaoService.GetUltimaAvaliacao().ToString());

            DataView dv = new DataView(dt);
            dv.RowFilter = "repeticao = "+repeticao;

            foreach (DataRowView row in dv)
            {

                var avaliacaoImagem = new Avaliacao_Imagem
                {

                    Imagem = row["imagem"].ToString(),     // byteArray.ToString(),
                    idAvaliacao = IdAvaliacao,
                    tratamento = int.Parse(row["tratamento"].ToString()),
                    repeticao = int.Parse(row["repeticao"].ToString()),
                    Data = DateTime.Now,
                    idUsuario = int.Parse(Settings.GeneralSettings)

                };

                try
                {
                    avaliacaoService.SalvarAvaliacaoImagem(avaliacaoImagem); ;


                    //alerta.SetTitle("Sucesso!");
                    //alerta.SetIcon(Android.Resource.Drawable.IcInputAdd);
                    //alerta.SetMessage("Imagem Salva com Sucesso!");
                    //alerta.SetButton("OK", (s, ev) =>
                    //{
                      //  alerta.Dismiss();
                    //});
                    //alerta.Show();

                }

                catch

                {
                    alerta.SetMessage("Erro ao salvar ");
                    alerta.SetTitle("ERRO!");
                    alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alerta.SetMessage("Erro ao salvar a Imagem!");
                    alerta.SetButton("OK", (s, ev) =>
                    {
                        alerta.Dismiss();
                    });
                    alerta.Show();
                }
            }
        }


        private void GetAlvos(int idTipoAvaliacao, int idEstudo)
        {
            alvos = new ArrayList();
            idAlvos = new ArrayList();
            AvaliacaoService aval = new AvaliacaoService();

            alvos.Add("Selecione");
            idAlvos.Add(0);

            var result = aval.GetAlvos(idTipoAvaliacao, idEstudo);

            foreach (var res in result)
            {
                string nome;
                if (res.Nome_vulgar.IndexOf(',') != -1)
                {
                    nome = res.Nome_vulgar.Substring(0, res.Nome_vulgar.IndexOf(','));
                }
                else
                {
                    nome = res.Nome_vulgar;
                }
                alvos.Add(nome);
                idAlvos.Add(res.IdAlvo);
            }

            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, alvos);
            spnAlvo.Adapter = adapter;

        }

    }
}


