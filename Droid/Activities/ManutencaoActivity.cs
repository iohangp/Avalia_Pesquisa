﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Avalia_Pesquisa.Droid.Helpers;
using Android.Views.InputMethods;




namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Manutenção")]
    public class ManutencaoActivity : BaseActivity
    {

        EditText textDose, textObservacoes, textVento, textNuvens, textUmidade, edNumEstudo, textTemperatura;

        ArrayAdapter adapterProduto, adapterTipoManutencao, adapterObjetivo, adapterUnidadeMedida;
        Spinner spinnerObjetivo, spinnerProduto, spinnerTipoManutencao, spinnerUnidadeMedida;
        ArrayList idObjetivo, Objetivo, idProduto, Produto, idTipoManutencao, TipoManutencao, idUnidadeMedida, UnidadeMedida;
        Button buttonSalvar, buttonScan, buttonValida;

        string idObjetivoSelect, idProdutoSelect, idTIpoManutencaoSelect, idUnidadeMedidaSelect;
        int idInstalacao, idPlanejamento, idEstudo_;
        double latitude = 0;
        double longitude = 0;

        protected override int LayoutResource => Resource.Layout.Manutencao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Oculta Teclado


            edNumEstudo = FindViewById<EditText>(Resource.Id.EDNumEstudo);
            buttonValida = FindViewById<Button>(Resource.Id.BTValidar);
            buttonScan = FindViewById<Button>(Resource.Id.BTScannerAvalia);
            buttonSalvar = FindViewById<Button>(Resource.Id.BTSalvarManutencao);
            spinnerProduto = FindViewById<Spinner>(Resource.Id.spnProduto);
            spinnerObjetivo = FindViewById<Spinner>(Resource.Id.spnObjetivo);
            spinnerTipoManutencao = FindViewById<Spinner>(Resource.Id.spnTipoManutencao);
            spinnerUnidadeMedida = FindViewById<Spinner>(Resource.Id.spnUnidadeMedida);
            textDose = FindViewById<EditText>(Resource.Id.etDose);
            textObservacoes = FindViewById<EditText>(Resource.Id.etObservacoesM);
            textVento = FindViewById<EditText>(Resource.Id.ETVelocidadeVentoM);
            textNuvens = FindViewById<EditText>(Resource.Id.ETPercentualNuvensM);
            textUmidade = FindViewById<EditText>(Resource.Id.ETUmidadeRelativaM);
            textTemperatura = FindViewById<EditText>(Resource.Id.ETTemperaturaM);


            buttonValida.Click += (sender, e) =>
            {
                ValidarEstudo(edNumEstudo.Text);
            };

            buttonScan.Click += BTScanner_Click;
            buttonSalvar.Click += BTSalvar_Click;

            GetProduto();
            GetManutencaoTipo();
            GetManutencaoObjetivo();
            GetUnidadeMedida();
            DadosMeterologicos();



            adapterTipoManutencao = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, TipoManutencao);
            spinnerTipoManutencao.Adapter = adapterTipoManutencao;

            adapterObjetivo = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Objetivo);
            spinnerObjetivo.Adapter = adapterObjetivo;

            adapterProduto = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Produto);
            spinnerProduto.Adapter = adapterProduto;

            adapterUnidadeMedida = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, UnidadeMedida);
            spinnerUnidadeMedida.Adapter = adapterUnidadeMedida;

            spinnerProduto.ItemSelected += SpnProduto_ItemSelected;
            spinnerObjetivo.ItemSelected += SpnObjetivo_ItemSelected;
            spinnerTipoManutencao.ItemSelected += SpnTipoManutencao_ItemSelected;
            spinnerUnidadeMedida.ItemSelected += SpnUnidadeMedida_ItemSelected;

            //InputMethodManager inputMnger = (InputMethodManager)this.GetSystemService(Context.InputMethodService); 
            //inputMnger.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            //OnTouchEvent(true);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(edNumEstudo.WindowToken, 0);
            return base.OnTouchEvent(e);
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

        protected internal void BTScanner_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(QrCodeActivity));
            StartActivityForResult(intent, 1);
        }


        private void GetProduto()
        {
            Produto = new ArrayList();
            idProduto = new ArrayList();
            ManutencaoService tas = new ManutencaoService();

            var result = tas.GetProduto();

            Produto.Add("Selecione");
            idProduto.Add(0);
            if (result != null)
            {
                foreach (var res in result)
                {
                    Produto.Add(res.Descricao);
                    idProduto.Add(res.idProdutos);
                }
            }

        }

        private void GetManutencaoObjetivo()
        {
            Objetivo = new ArrayList();
            idObjetivo = new ArrayList();
            ManutencaoService tas = new ManutencaoService();

            var result = tas.GetManutencaoObjetivo();

            Objetivo.Add("Selecione");
            idObjetivo.Add(0);
            if (result != null)
            {
                foreach (var res in result)
                {
                    Objetivo.Add(res.Descricao);
                    idObjetivo.Add(res.idManutencao_Objetivo);
                }
            }
        }
        private void GetManutencaoTipo()
        {
            TipoManutencao = new ArrayList();
            idTipoManutencao = new ArrayList();
            ManutencaoService tas = new ManutencaoService();

            var result = tas.GetManutencaoTipo();

            TipoManutencao.Add("Selecione");
            idTipoManutencao.Add(0);
            if (result != null)
            {
                foreach (var res in result)
                {
                    TipoManutencao.Add(res.Descricao);
                    idTipoManutencao.Add(res.idManutencao_Tipo);
                }
            }
        }

        private void GetUnidadeMedida()
        {
            UnidadeMedida = new ArrayList();
            idUnidadeMedida = new ArrayList();
            ManutencaoService tas = new ManutencaoService();

            var result = tas.GetUnidadeMedida();

            UnidadeMedida.Add("Selecione");
            idUnidadeMedida.Add(0);
            if (result != null)
            {
                foreach (var res in result)
                {
                    UnidadeMedida.Add(res.Descricao);
                    idUnidadeMedida.Add(res.idUnidade_Medida);
                }
            }
        }

        protected internal void BTSalvar_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();
            ManutencaoService manuService = new ManutencaoService();

            if (int.Parse(idObjetivoSelect) > 0 && int.Parse(idTIpoManutencaoSelect) > 0 && int.Parse(idUnidadeMedidaSelect) > 0 &&
                idEstudo_ > 0 && textDose.Text != "")
            {
                string Velocidade_Vento = "0";
                if (textVento.Text == "")
                    Velocidade_Vento = "0";
                else Velocidade_Vento = textVento.Text.Replace("km/h", "");

                decimal Umidade_Relativa = 0;
                if (textUmidade.Text == "")
                    Umidade_Relativa = 0;
                else Umidade_Relativa = decimal.Parse(textUmidade.Text.Replace("%", ""));

                decimal Percentual_Nuvens = 0;
                if (textNuvens.Text == "")
                    Percentual_Nuvens = 0;
                else Percentual_Nuvens = decimal.Parse(textNuvens.Text.Replace("%", ""));


                var manutencao = new Manutencao
                {

                    idInstalacao = idInstalacao,
                    idManutencao_Tipo = Convert.ToInt32(idTIpoManutencaoSelect),
                    Data = DateTime.Now,
                    idProduto = Convert.ToInt32(idProdutoSelect),
                    idUsuario = Convert.ToInt32(Settings.GeneralSettings),
                    Dose = decimal.Parse(textDose.Text.Replace(".", ",")),
                    idUnidade_Medida = Convert.ToInt32(idUnidadeMedidaSelect),
                    idManutencao_Objetivo = Convert.ToInt32(idObjetivoSelect),
                    Hora_Inicio_Fim = DateTime.Now,
                    Temperatura = textTemperatura.Text,
                    Umidade_Relativa = Umidade_Relativa,
                    Velocidade_Vento = Convert.ToDecimal(Velocidade_Vento.Replace(".", ",")),
                    Percentual_Nuvens = Percentual_Nuvens,
                    Observacoes = textObservacoes.Text,
                    Longitude = longitude.ToString(),
                    Latitude = latitude.ToString()

                    //idInstalacao = 1,
                    //idManutencao_Tipo = 1,
                    //Data = DateTime.Now,
                    //idProduto = 1,
                    //idUsuario = 3,
                    //Dose = 1,
                    //idUnidade_Medida = 4,
                    //idManutencao_Objetivo = 1,
                    //Hora_Inicio_Fim = DateTime.Now,
                    //Temperatura = "10",
                    //Umidade_Relativa = 1,
                    //Velocidade_Vento = 10,
                    //Percentual_Nuvens = 1,
                    //Observacoes = "texto",
                    //Longitude = "3232323",
                    //Latitude = "3232323"

                };

                try
                {
                    if (manuService.SalvarManutencao(manutencao) == true)
                    {
                        alerta.SetTitle("Sucesso!");
                        alerta.SetIcon(Android.Resource.Drawable.IcInputAdd);
                        alerta.SetMessage("Manutenção Salva com Sucesso!");
                        alerta.SetButton("OK", (s, ev) =>
                        {
                            alerta.Dismiss();
                        });
                        alerta.Show();
                        LimparCampos();
                    }

                    else

                    {
                        alerta.SetMessage("Erro ao salvar ");
                        alerta.SetTitle("ERRO!");
                        alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                        alerta.SetMessage("Erro ao salvar a Manutenção!");
                        alerta.SetButton("OK", (s, ev) =>
                        {
                            alerta.Dismiss();
                        });
                        alerta.Show();
                    }
                }

                catch

                {
                    alerta.SetMessage("Erro ao salvar ");
                    alerta.SetTitle("ERRO!");
                    alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alerta.SetMessage("Erro ao salvar a Manutenção!");
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
                alerta.SetMessage("Favor preencher todos os campos obrigatorios!");
                alerta.SetButton("OK", (s, ev) =>
                {
                    alerta.Dismiss();
                });
                alerta.Show();


            }


        }

        private void SpnObjetivo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idObjetivoSelect = idObjetivo[e.Position].ToString();

        }

        private void SpnTipoManutencao_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idTIpoManutencaoSelect = idTipoManutencao[e.Position].ToString();

        }

        private void SpnProduto_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idProdutoSelect = idProduto[e.Position].ToString();

        }
        private void SpnUnidadeMedida_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idUnidadeMedidaSelect = idUnidadeMedida[e.Position].ToString();

        }
        private void LimparCampos()
        {


            spinnerProduto.SetSelection(0);
            spinnerObjetivo.SetSelection(0);
            spinnerTipoManutencao.SetSelection(0);
            spinnerUnidadeMedida.SetSelection(0);
            textDose.Text = textObservacoes.Text = textVento.Text = textNuvens.Text = textUmidade.Text = edNumEstudo.Text = textTemperatura.Text = "";


            idProdutoSelect = "";
            idObjetivoSelect = "";
            idTIpoManutencaoSelect = "";
            idUnidadeMedidaSelect = "";
            idInstalacao = idPlanejamento = idEstudo_ = 0;
            latitude = longitude = 0;




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

        private void ValidarEstudo(string protocolo)
        {
            string[] ids = new string[2]; ;

            if (protocolo.IndexOf('|') != -1)
                ids = protocolo.Split('|');
            else
                ids[0] = protocolo;

            ConsultaEstudoService ces = new ConsultaEstudoService();
            var estudo = ces.GetEstudo(ids[0]);
            buttonSalvar.Visibility = ViewStates.Visible;

            if (estudo.Count > 0)
            {
                if (estudo[0].idInstalacao != 0)
                {
                    idEstudo_ = estudo[0].IdEstudo;
                    idInstalacao = estudo[0].idInstalacao;
                    edNumEstudo.Text = estudo[0].Codigo;
                    DadosMeterologicos();
                    Coordenadas();
                }
                else
                {
                    LimparCampos();
                    Toast.MakeText(this, "O estudo não contem uma intalação relacionada", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Nenhum estudo encontrado", ToastLength.Long).Show();
            }



        }

        public async void Coordenadas()
        {

            var locator = Plugin.Geolocator.CrossGeolocator.Current;

            Plugin.Geolocator.Abstractions.Position position = null;
            try
            {
                locator.DesiredAccuracy = 100;

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);
                longitude = position.Longitude;
                latitude = position.Latitude;



            }

            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine(ex.Message);
            }
        }

        public async void DadosMeterologicos()
        {
            try
            {
                Weather weather = await WeatherService.GetWeather();
                if (weather.Title != "")
                {
                    //FindViewById<TextView>(Resource.Id.locationText).Text = weather.Title;
                    textTemperatura.Text = weather.Temperature;
                    textVento.Text = weather.Wind;
                    textNuvens.Text = weather.Clouds;
                    textUmidade.Text = weather.Humidity;

                }
                else
                {
                    Toast.MakeText(this, "Sem conexão para obter dados climáticos!", ToastLength.Long).Show();
                }
            }

            catch
            {
                Toast.MakeText(this, "Sem conexão para obter dados climáticos!", ToastLength.Long).Show();
            }

        }




    }

}
