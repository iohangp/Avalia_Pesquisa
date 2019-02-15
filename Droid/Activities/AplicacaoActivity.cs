
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
    [Activity(Label = "Aplicação")]
    public class AplicacaoActivity : BaseActivity
    {

        EditText textBBCH, textObservacoes, textVento, textNuvens, textUmidade, edNumEstudo, textTemperatura, textVolumeChuva;
        TextView textDate, textChuva;
        ArrayAdapter adapter;
        Spinner spinnerEquipamento;
        ArrayList idEquipamento, Equipamento;
        Button buttonSalvar, buttonScan, buttonValida;
        ImageButton buttonCalendarAplicacao, buttonDataChuva;
        string idEquipamentoSelect;
        int idInstalacao, idPlanejamento, idEstudo_;
        double latitude = 0;
        double longitude = 0;

        protected override int LayoutResource => Resource.Layout.Aplicacao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            //idEquipamento = new ArrayList();
            //Equipamento = new ArrayList();
            // Equipamento.Add("Selecione");
            // idEquipamento.Add(0);

            buttonValida = FindViewById<Button>(Resource.Id.BTValidar);
            buttonScan = FindViewById<Button>(Resource.Id.BTScannerAvalia);
            buttonSalvar = FindViewById<Button>(Resource.Id.BTSalvarAplicacao);
            spinnerEquipamento = FindViewById<Spinner>(Resource.Id.SPNEquipamento);
            textBBCH = FindViewById<EditText>(Resource.Id.ETBBCH);
            textObservacoes = FindViewById<EditText>(Resource.Id.ETObservacoes);
            textVento = FindViewById<EditText>(Resource.Id.ETVento);
            textNuvens = FindViewById<EditText>(Resource.Id.ETPercentual);
            textVolumeChuva = FindViewById<EditText>(Resource.Id.ETChuva);
            textUmidade = FindViewById<EditText>(Resource.Id.ETUmidadeRelativa);
            edNumEstudo = FindViewById<EditText>(Resource.Id.EDNumEstudo);
            textTemperatura = FindViewById<EditText>(Resource.Id.ETTemperatura);
            spinnerEquipamento = FindViewById<Spinner>(Resource.Id.SPNEquipamento);
            textDate = FindViewById<TextView>(Resource.Id.TVDataAplicacao);
            textChuva = FindViewById<TextView>(Resource.Id.TVDataChuva);
            buttonCalendarAplicacao = FindViewById<ImageButton>(Resource.Id.IBCalendarAplicacao);
            buttonDataChuva = FindViewById<ImageButton>(Resource.Id.IBCalendarChuva);

            buttonCalendarAplicacao.Click += DateSelect_OnClick;
            buttonDataChuva.Click += ChuvaSelect_OnClick;

            buttonValida.Click += (sender, e) =>
            {
                ValidarEstudo(edNumEstudo.Text);
            };

            buttonScan.Click += BTScanner_Click;
            buttonSalvar.Click += BTSalvar_Click;

            GetEquipamento();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Equipamento);
            spinnerEquipamento.Adapter = adapter;

            spinnerEquipamento.ItemSelected += SpnEquipamento_ItemSelected;

            DadosMeterologicos();

        }


        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                textDate.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        void ChuvaSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time) {
                textChuva.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
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

        private void GetEquipamento()
        {
            Equipamento = new ArrayList();
            idEquipamento = new ArrayList();
            AplicacaoService tas = new AplicacaoService();

            var result = tas.GetEquipamento();

            Equipamento.Add("Selecione");
            idEquipamento.Add(0);
            foreach (var res in result)
            {
                Equipamento.Add(res.Descricao);
                idEquipamento.Add(res.IdEquipamento);
            }

        }


        protected internal void BTSalvar_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();
            AplicacaoService apliService = new AplicacaoService();


            if (idEstudo_ > 0)
            {
                var aplicacao = new Aplicacao
                {

                    idInstalacao = 1,
                    Data_Aplicacao = DateTime.Parse(textDate.Text),
                    Umidade_Relativa = decimal.Parse(textUmidade.Text.Replace("%", "")),
                    Temperatura = textTemperatura.Text,
                    Velocidade_Vento = decimal.Parse(textVento.Text.Replace("km/h", "")),
                    Percentual_nuvens = decimal.Parse(textNuvens.Text.Replace("%", "")),
                    Chuva_Data = DateTime.Parse(textChuva.Text),
                    Chuva_Volume = decimal.Parse(textVolumeChuva.Text),
                    IdEquipamento = int.Parse(idEquipamentoSelect),
                    BBCH = decimal.Parse(textBBCH.Text),
                    Observacoes = textObservacoes.Text,
                    Longitude = longitude.ToString(),
                    Latitude = latitude.ToString(),
                    Data_Realizada = DateTime.Now,
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

                         else {
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

        private void SpnEquipamento_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idEquipamentoSelect = idEquipamento[e.Position].ToString();

        }

        private void LimparCampos()
        {


            spinnerEquipamento.SetSelection(0);

            textBBCH.Text = textObservacoes.Text = textVento.Text = textNuvens.Text = textUmidade.Text = edNumEstudo.Text = textTemperatura.Text = textVolumeChuva.Text = "";
            textDate.Text = textChuva.Text = "";

             idEquipamentoSelect = "";
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
                idEstudo_ = estudo[0].IdEstudo;
                edNumEstudo.Text = estudo[0].Codigo;
                DadosMeterologicos();
                Coordenadas();
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

            catch {
                Toast.MakeText(this, "Sem conexão para obter dados climáticos!", ToastLength.Long).Show();
            }

        }


    }

}