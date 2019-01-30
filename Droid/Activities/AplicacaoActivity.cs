﻿
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
        ImageButton buttonCalendar;

        EditText textBBCH, textObservacoes, textVento, textNuvens, textUmidade, edNumEstudo, textTemperatura;
        TextView textDate;
        Spinner spinnerEquipamento;
        Button buttonSalvar, buttonValida;
        string idEquipamentoSelect;
        int totalrepeticoes = 1, idEstudo, idPlanejamento;

        protected override int LayoutResource => Resource.Layout.Aplicacao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Aplicacao);

            buttonValida = FindViewById<Button>(Resource.Id.BTValidar); 
        //    spinnerEquipamento = FindViewById<Spinner>(Resource.Id.SPNEquipamento);
        //   textBBCH = FindViewById<EditText>(Resource.Id.etBBCH);
        //    textObservacoes = FindViewById<EditText>(Resource.Id.etObservações);

            //         buttonCalendar.Click += DateSelect_OnClick;
            //       buttonSalvar.Click += BTSalvar_Click;

            buttonValida.Click += (sender, e) =>
            {
                //ValidarEstudo(edNumEstudo.Text);
            };
        }


        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                textDate.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        //Botao Voltar do celular
        public override void OnBackPressed()
        {
            Finish();
        }


        protected internal void BTSalvar_Click(object sender, EventArgs e)
        {

            //AplicacaoService apliService = new AplicacaoService();

            var aplicacao = new Aplicacao
            {
                // IdEstudo = idEstudo,
                idInstalacao = 1,
                Data_Aplicacao = DateTime.Parse(textDate.Text),
                Umidade_Relativa = decimal.Parse(textTemperatura.Text),
                Temperatura = decimal.Parse(textTemperatura.Text),
                Velocidade_Vento = decimal.Parse(textVento.Text),
                Percentual_nuvens = decimal.Parse(textNuvens.Text),
                IdEquipamento = int.Parse(idEquipamentoSelect),
                BBCH = decimal.Parse(textBBCH.Text),
                Observacoes = textObservacoes.Text
            };
       //     apliService.SalvarAplicacao(aplicacao);


        }


        public async void DadosMeterologicos()
        {


            // EditText zipCodeEntry = FindViewById<EditText>(Resource.Id.ZipCodeEntry);

            //  if (!String.IsNullOrEmpty(zipCodeEntry.Text))
            // {
            Weather weather = await WeatherService.GetWeather(); //zipCodeEntry.Text
            //if (weather.Title != "")
            //{

            //    FindViewById<EditText>(Resource.Id.etTemperatura).Text = weather.Temperature;
            //    FindViewById<EditText>(Resource.Id.etVento).Text = weather.Wind;

            //    FindViewById<EditText>(Resource.Id.etUmidade).Text = weather.Humidity;
            //    FindViewById<EditText>(Resource.Id.etNuvens).Text = weather.Clouds;

            //}
            //   else
            //   {
            //       Toast.MakeText(this, "Localização não encontrada!", ToastLength.Long).Show();
            //}

        }


    }

    //private void ValidarEstudo2(string protocolo)
    //{
    //    int numRepeticao = 1;



    //    ConsultaEstudoService ces = new ConsultaEstudoService();
    //    var estudo = ces.GetEstudo(protocolo);

    //    if (estudo.Count > 0)
    //    {

    //        idEstudo = estudo[0].IdEstudo;


    //        AplicacaoService aval = new AplicacaoService();
    //        var plan = aval.GetDataAplicacao(idEstudo);


    //        buttonSalvar.Visibility = ViewStates.Visible;



    //        AlertDialog.Builder builder = new AlertDialog.Builder(this);
    //        AlertDialog alerta = builder.Create();

    //        alerta.SetTitle("Atenção!");
    //        alerta.SetIcon(Android.Resource.Drawable.IcDelete);
    //        alerta.SetMessage("Todas as aplicações para este estudo já foram realizadas");
    //        alerta.SetButton("OK", (s, ev) =>
    //        {
    //            alerta.Dismiss();
    //        });
    //        alerta.Show();

    //    }
    //    else
    //    {
    //        //EscondeCampos();
    //        Toast.MakeText(this, "Nenhum estudo encontrado", ToastLength.Long).Show();
    //    }


    //}

}
