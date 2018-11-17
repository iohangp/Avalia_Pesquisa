
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Avaliar Estudo")]
    public class AvaliacaoActivity : BaseActivity
    {
        Spinner spnTipo;
        ArrayAdapter adapter;
        ArrayList tipos;
        EditText edNumEstudo, etRepeticao1, etRepeticao2, etRepeticao3, etRepeticao4, etRepeticao5;
        int numRepeticao = 1;
        TableRow rowRepeticao1,rowRepeticao2,rowRepeticao3,rowRepeticao4,rowRepeticao5;
        Button buttonSalvar;

        protected override int LayoutResource => Resource.Layout.Avaliacao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //  SetContentView(Resource.Layout.Avaliacao);
            // Create your application here
            spnTipo = FindViewById<Spinner>(Resource.Id.spnTipoAvaliacao);
            edNumEstudo = FindViewById<EditText>(Resource.Id.EDNumEstudo);
            Button buttonValida = FindViewById<Button>(Resource.Id.BTValidar);
            Button buttonScan = FindViewById<Button>(Resource.Id.BTScannerAvalia);

            tipos = GetAvaliacaoTipo();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, tipos);
            //vincula o adaptador ao controle spinner
            spnTipo.Adapter = adapter;

            buttonScan.Click += BTScanner_Click;

            buttonValida.Click += (sender, e) =>
            {
                ValidarEstudo(edNumEstudo.Text);
            };


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

        private void ValidarEstudo(string protocolo)
        {
            numRepeticao = 1;
            rowRepeticao1 = FindViewById<TableRow>(Resource.Id.trRepeticao1);
            rowRepeticao2 = FindViewById<TableRow>(Resource.Id.trRepeticao2);
            rowRepeticao3 = FindViewById<TableRow>(Resource.Id.trRepeticao3);
            rowRepeticao4 = FindViewById<TableRow>(Resource.Id.trRepeticao4);
            rowRepeticao5 = FindViewById<TableRow>(Resource.Id.trRepeticao5);
            buttonSalvar = FindViewById<Button>(Resource.Id.BTSalvarAvaliacao);

            etRepeticao1 = FindViewById<EditText>(Resource.Id.etRepeticao1);
            etRepeticao2 = FindViewById<EditText>(Resource.Id.etRepeticao2);
            etRepeticao3 = FindViewById<EditText>(Resource.Id.etRepeticao3);
            etRepeticao4 = FindViewById<EditText>(Resource.Id.etRepeticao4);
            etRepeticao5 = FindViewById<EditText>(Resource.Id.etRepeticao5);

            ConsultaEstudoService ces = new ConsultaEstudoService();
            var estudo = ces.GetEstudo(protocolo);

            if(estudo.Count > 0) {
                while (estudo[0].Repeticao >= numRepeticao)
                {
                    if(numRepeticao == 1)
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
                etRepeticao1.Text = etRepeticao2.Text = etRepeticao3.Text = etRepeticao4.Text = etRepeticao5.Text = "";
                rowRepeticao1.Visibility = ViewStates.Invisible;
                rowRepeticao2.Visibility = ViewStates.Invisible;
                rowRepeticao3.Visibility = ViewStates.Invisible;
                rowRepeticao4.Visibility = ViewStates.Invisible;
                rowRepeticao5.Visibility = ViewStates.Invisible;

                buttonSalvar.Visibility = ViewStates.Invisible;
                Toast.MakeText(this, "Nenhum estudo encontrado", ToastLength.Long).Show();
            }


        }

        private ArrayList GetAvaliacaoTipo()
        {
            ArrayList tipoavaliacao = new ArrayList();
            TipoAvaliacaoService tas = new TipoAvaliacaoService();

            var result = tas.GetAvaliacaoTipo();

            foreach (var res in result)
            {
                tipoavaliacao.Add(res.Descricao);
            }

            return tipoavaliacao;

        }
    }
}
