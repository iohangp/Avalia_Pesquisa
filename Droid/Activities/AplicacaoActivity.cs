
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

        EditText textBBCH, textObservacoes;
        TextView textDate;
        Spinner spinnerEquipamento;
        Button buttonSalvar;
        string idEquipamentoSelect;

        protected override int LayoutResource => Resource.Layout.Aplicacao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Aplicacao);

            spinnerEquipamento = FindViewById<Spinner>(Resource.Id.SPNEquipamento);
            textBBCH = FindViewById<EditText>(Resource.Id.etBBCH);
            textObservacoes = FindViewById<EditText>(Resource.Id.etObservações);

            buttonCalendar.Click += DateSelect_OnClick;
            buttonSalvar.Click += BTSalvar_Click;
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


        }

    }
}