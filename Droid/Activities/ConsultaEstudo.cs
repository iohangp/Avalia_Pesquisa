using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Avalia_Pesquisa.Droid.Activities;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using System.Threading;
using Avalia_Pesquisa.Droid.Helpers;
using Android.Content.PM;
using static Android.Support.V7.Widget.RecyclerView;
using Android.Text;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Consultar Estudo")]
    public class ConsultaEstudo : BaseActivity
    {

        ViewPager pager;
        TabsAdapter adapter;
        DataBase db;
        CloudDataStore CloudData;
        TextView txtEstudo, txtProtocolo, txtCultura, txtPatrocinador, txtProduto,
                 txtClasse, txtAlvo, txtRepeticao, txtIntervAplicacao, txtTratSementes,
                 txtVolumeCalda, txtObjetivo, txtRet, txtValRet, txtFaseRet, txtObs, txtResponsavel;

        protected override int LayoutResource => Resource.Layout.ConsultaEstudo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // SetContentView(Resource.Layout.ConsultaEstudo);
            txtEstudo = FindViewById<TextView>(Resource.Id.TVNumeroEstudo);
            Button buttonConsultar2 = FindViewById<Button>(Resource.Id.BTConsultar2);
            Button buttonScanner = FindViewById<Button>(Resource.Id.BTScanner);

            // Create your application here

            buttonConsultar2.Click += (sender, e) =>
            {
                ConsultarEstudo(txtEstudo.Text);
            };

            buttonScanner.Click += BTScanner_Click;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            txtEstudo = FindViewById<TextView>(Resource.Id.TVNumeroEstudo);

            if (requestCode == 1)
            {
                if (resultCode == Result.Ok)
                {
                    if (data.GetStringExtra("qrcode") != null)
                    {
                        //edNumEstudo.Text = data.GetStringExtra("qrcode");
                        //ValidarEstudo(edNumEstudo.Text);
                        string codigo = data.GetStringExtra("qrcode");
                        ConsultarEstudo(codigo);

                    }
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        public void ConsultarEstudo(string protocolo)
        {
            txtProtocolo = FindViewById<TextView>(Resource.Id.TVProtocolo);
            txtPatrocinador = FindViewById<TextView>(Resource.Id.TVPatrocinador);
            txtProduto = FindViewById<TextView>(Resource.Id.TVProduto);
            txtCultura = FindViewById<TextView>(Resource.Id.TVCultura);
            txtClasse = FindViewById<TextView>(Resource.Id.TVClasse);
            //     txtAlvo = FindViewById<TextView>(Resource.Id.TVAlvo);
            txtRepeticao = FindViewById<TextView>(Resource.Id.TVRepeticao);
            txtIntervAplicacao = FindViewById<TextView>(Resource.Id.TVIntervAplicacao);
            txtTratSementes = FindViewById<TextView>(Resource.Id.TVTratSementes);
            txtVolumeCalda = FindViewById<TextView>(Resource.Id.TVVolumeCalda);
            txtObjetivo = FindViewById<TextView>(Resource.Id.TVObjetivo);
            txtRet = FindViewById<TextView>(Resource.Id.TVRET);
            txtFaseRet = FindViewById<TextView>(Resource.Id.TVFaseRET);
            txtValRet = FindViewById<TextView>(Resource.Id.TVValidadeRET);
            txtObs = FindViewById<TextView>(Resource.Id.TVObs);
            txtResponsavel = FindViewById<TextView>(Resource.Id.TVResponsavel);

            string[] ids = new string[3];

            if (!string.IsNullOrEmpty(protocolo))
            {

                if (protocolo.IndexOf('|') != -1)
                    ids = protocolo.Split('|');
                else
                    ids[0] = protocolo;



                ConsultaEstudoService ces = new ConsultaEstudoService();
                var estudo = ces.GetEstudo(ids[0]);
                if (estudo.Count > 0)
                {
                    txtEstudo.Text = estudo[0].Protocolo;
                    txtProtocolo.Text = estudo[0].Codigo;
                    txtPatrocinador.Text = estudo[0].Cliente;
                    txtProduto.Text = estudo[0].Produto;
                    txtCultura.Text = estudo[0].Cultura;
                    txtClasse.Text = estudo[0].Classe;
                    // txtAlvo.Text = estudo[0].Alvo;
                    txtRepeticao.Text = estudo[0].Repeticao.ToString();
                    txtIntervAplicacao.Text = estudo[0].Intervalo_Aplicacao.ToString();
                    txtTratSementes.Text = estudo[0].Tratamento_Sementes.ToString();
                    txtVolumeCalda.Text = estudo[0].Volume_Calda.ToString();
                    txtObjetivo.Text = estudo[0].Objetivo;
                    txtRet.Text = estudo[0].RET;
                    txtFaseRet.Text = estudo[0].RET_Fase.ToString();

                    DateTime data = estudo[0].Validade_RET;
                    if (data.Year > 1)
                        txtValRet.Text = string.Format("{0:dd/MM/yyyy}", data);

                    txtObs.Text = estudo[0].Observacoes;
                    txtResponsavel.Text = estudo[0].Responsavel;


                }
                else
                {
                    txtProtocolo.Text = txtPatrocinador.Text = txtProduto.Text = txtCultura.Text = "";
                    txtClasse.Text = txtRepeticao.Text = txtIntervAplicacao.Text = "";
                    txtTratSementes.Text = txtVolumeCalda.Text = txtObjetivo.Text = txtRet.Text = txtFaseRet.Text = "";
                    txtValRet.Text = txtObs.Text = txtResponsavel.Text = "";
                    Toast.MakeText(this, "Nenhum estudo encontrado", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Informe o código ou utilize a função Scanner", ToastLength.Long).Show();
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

        public override void OnBackPressed()
        {
            Finish();
        }

        protected internal void BTScanner_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(QrCodeActivity));
            StartActivityForResult(intent, 1);
        }

    }
}

