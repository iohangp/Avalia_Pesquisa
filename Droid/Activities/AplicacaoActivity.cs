
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

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Aplicação")]
    public class AplicacaoActivity : BaseActivity
    {

        EditText textBBCH, textObservacoes, textVento, textNuvens, textUmidade, edNumEstudo, textTemperatura;
        TextView textDate, textChuva;
        ArrayAdapter adapter;
        Spinner spinnerEquipamento;
        ArrayList idEquipamento, Equipamento;
        Button buttonSalvar, buttonScan, buttonValida;
        ImageButton buttonCalendarAplicacao, buttonDataChuva;
        string idEquipamentoSelect;
        int idEstudo, idPlanejamento, idEstudo_;

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

            AplicacaoService apliService = new AplicacaoService();

            var aplicacao = new Aplicacao
            {
                 IdAplicacao = idEstudo,
                idInstalacao = 1,
                Data_Aplicacao = DateTime.Parse(textDate.Text),
                Umidade_Relativa = decimal.Parse(textTemperatura.Text),
                Temperatura = decimal.Parse(textTemperatura.Text),
                Velocidade_Vento = decimal.Parse(textVento.Text),
                Percentual_nuvens = decimal.Parse(textNuvens.Text),
                IdEquipamento = int.Parse(idEquipamentoSelect),
                BBCH = decimal.Parse(textBBCH.Text),
                Observacoes = textObservacoes.Text,
                idUsuario = 1
            };
            apliService.SalvarAplicacao(aplicacao);


        }

        private void SpnEquipamento_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idEquipamentoSelect = idEquipamento[e.Position].ToString();

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
            }

        }



        public async void DadosMeterologicos()
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


    }

}