
using System;
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

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Manutenção")]
    public class ManutencaoActivity : BaseActivity
    {

        EditText textDose, textObservacoes, textVento, textNuvens, textUmidade, edNumEstudo, textTemperatura; 

        ArrayAdapter adapter;
        Spinner spinnerObjetivo, spinnerProduto, spinnerTipoManutencao;
        ArrayList idObjetivo, Objetivo, idProduto, Produto, idTipoManutencao, TipoManutencao;
        Button buttonSalvar, buttonScan, buttonValida;

        string idObjetivoSelect, idProdutoSelect, idTIpoManutencaoSelect;
        int idInstalacao, idPlanejamento, idEstudo_;
        double latitude = 0;
        double longitude = 0;

        protected override int LayoutResource => Resource.Layout.Aplicacao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);



            buttonValida = FindViewById<Button>(Resource.Id.BTValidar);
            buttonScan = FindViewById<Button>(Resource.Id.BTScannerAvalia);
            buttonSalvar = FindViewById<Button>(Resource.Id.BTSalvarAplicacao);
            spinnerProduto = FindViewById<Spinner>(Resource.Id.spnProduto);
            spinnerObjetivo = FindViewById<Spinner>(Resource.Id.spnObjetivo);
            spinnerTipoManutencao = FindViewById<Spinner>(Resource.Id.spnTipoManutencao);

            textDose = FindViewById<EditText>(Resource.Id.etDose);
            textObservacoes = FindViewById<EditText>(Resource.Id.ETObservacoes);
            textVento = FindViewById<EditText>(Resource.Id.ETVento);
            textNuvens = FindViewById<EditText>(Resource.Id.ETPercentual);
            textUmidade = FindViewById<EditText>(Resource.Id.ETUmidadeRelativa);
            edNumEstudo = FindViewById<EditText>(Resource.Id.EDNumEstudo);
            textTemperatura = FindViewById<EditText>(Resource.Id.ETTemperatura);





            buttonValida.Click += (sender, e) =>
            {
                ValidarEstudo(edNumEstudo.Text);
            };

            buttonScan.Click += BTScanner_Click;
            buttonSalvar.Click += BTSalvar_Click;

            GetProduto();
            GetManutencaoTipo();
            GetManutencaoObjetivo();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Produto);
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, TipoManutencao);
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Objetivo);
            spinnerProduto.Adapter = adapter;
            spinnerObjetivo.Adapter = adapter;
            spinnerTipoManutencao.Adapter = adapter;

            spinnerProduto.ItemSelected += SpnProduto_ItemSelected;
            spinnerObjetivo.ItemSelected += SpnObjetivo_ItemSelected;
            spinnerTipoManutencao.ItemSelected += SpnTipoManutencao_ItemSelected;
            DadosMeterologicos();

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
            foreach (var res in result)
            {
                Produto.Add(res.Descricao);
                idProduto.Add(res.idProduto);
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
            foreach (var res in result)
            {
                Objetivo.Add(res.Descricao);
                idObjetivo.Add(res.idManutencao_Objetivo);
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
            foreach (var res in result)
            {
                Produto.Add(res.Descricao);
                idProduto.Add(res.idManutencao_Tipo);
            }

        }

        protected internal void BTSalvar_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();
            AplicacaoService apliService = new AplicacaoService();


            if (idEstudo_ > 0)
            {
                var manutencao = new Manutencao
                {

                    idInstalacao = 1,

                    Umidade_Relativa = decimal.Parse(textUmidade.Text.Replace("%", "")),
                    Temperatura = textTemperatura.Text,
                    Velocidade_Vento = decimal.Parse(textVento.Text.Replace("km/h", "")),
                    Percentual_Nuvens = decimal.Parse(textNuvens.Text.Replace("%", "")),
                    Dose = decimal.Parse(textDose.Text),
                    idProduto = int.Parse(idProdutoSelect),
                    idManutencao_Objetivo = int.Parse(idProdutoSelect),
                    idManutencao_Tipo = int.Parse(idTIpoManutencaoSelect),

                    Observacoes = textObservacoes.Text,
                    Longitude = longitude.ToString(),
                    Latitude = latitude.ToString(),
                    Data = DateTime.Now,
                    idUsuario = int.Parse(Settings.GeneralSettings)

    };

                try
                {
                    apliService.SalvarAplicacao(aplicacao); ;


                    alerta.SetTitle("Sucesso!");
                    alerta.SetIcon(Android.Resource.Drawable.IcInputAdd);
                    alerta.SetMessage("Instalação Salva com Sucesso!");
                    alerta.SetButton("OK", (s, ev) =>
                    {
                        alerta.Dismiss();
                    });
                    alerta.Show();
                    LimparCampos();
                }

                catch

                {
                    alerta.SetMessage("Erro ao salvar ");
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
                alerta.SetMessage("Favor informar um estudo válido ");
                alerta.SetTitle("ERRO!");
                alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                alerta.SetMessage("Favor informar um estudo válido!");
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

        private void LimparCampos()
        {


            spinnerProduto.SetSelection(0);
            spinnerObjetivo.SetSelection(0);
            spinnerTipoManutencao.SetSelection(0);
            textDose.Text = textObservacoes.Text = textVento.Text = textNuvens.Text = textUmidade.Text = edNumEstudo.Text = textTemperatura.Text = "";


            idProdutoSelect = "";
            idObjetivoSelect = "";
            idTIpoManutencaoSelect = "";
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
                        edNumEstudo.Text = data.GetStringExtra("qrcode");
                        ValidarEstudo(edNumEstudo.Text);
                    }
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        private void ValidarEstudo(string protocolo)
        {
            string[] ids = protocolo.Split('-');

            ConsultaEstudoService ces = new ConsultaEstudoService();
            var estudo = ces.GetEstudo(int.Parse(ids[0]));
            buttonSalvar.Visibility = ViewStates.Visible;

            if (estudo.Count > 0)
            {
                idEstudo_ = estudo[0].IdEstudo;
                DadosMeterologicos();
                Coordenadas();
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