
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
using Avalia_Pesquisa.Droid.Helpers;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Avaliar Estudo")]
    public class AvaliacaoActivity : BaseActivity
    {
            Spinner spnTipo;
            ArrayAdapter adapter;
            ArrayList tipos, idTipos;
            EditText edNumEstudo, etRepeticao1, etRepeticao2, etRepeticao3, etRepeticao4, etRepeticao5;
            int totalRepeticoes = 1, idEstudo;
            string idTipoAvaliacao;
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
            buttonSalvar = FindViewById<Button>(Resource.Id.BTSalvarAvaliacao); 
            Button buttonValida = FindViewById<Button>(Resource.Id.BTValidar);
            Button buttonScan = FindViewById<Button>(Resource.Id.BTScannerAvalia);

            GetAvaliacaoTipo();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, tipos);
            //vincula o adaptador ao controle spinner
            spnTipo.Adapter = adapter;

            buttonScan.Click += BTScanner_Click;
            buttonSalvar.Click += BTSalvar_Click;

            buttonValida.Click += (sender, e) =>
            {
                ValidarEstudo(edNumEstudo.Text);
            };

            spnTipo.ItemSelected += SpnTipo_ItemSelected;


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

        private void SpnTipo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idTipoAvaliacao = idTipos[e.Position].ToString();
        }

        protected internal void BTScanner_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(QrCodeActivity));
            StartActivityForResult(intent, 1);
        }

        protected internal void BTSalvar_Click(object sender, EventArgs e)
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

            if (sucesso) { 
                AvaliacaoService avalService = new AvaliacaoService();
                if (etRepeticao1.Text != "")
                {
                    var aval = new Avaliacao
                    {
                        IdEstudo = idEstudo,
                        Repeticao = 1,
                        Valor = decimal.Parse(etRepeticao1.Text.Replace(".", ",")),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Data = DateTime.Now,
                        idAvaliacao_Tipo = int.Parse(idTipoAvaliacao)
                    };
                    if (!avalService.SalvarAvaliacao(aval))
                        sucesso = false;
                   
                }
                if (etRepeticao2.Text != "")
                {
                    var aval = new Avaliacao
                    {
                        IdEstudo = idEstudo,
                        Repeticao = 2,
                        Valor = decimal.Parse(etRepeticao2.Text.Replace(".", ",")),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Data = DateTime.Now,
                        idAvaliacao_Tipo = int.Parse(idTipoAvaliacao)
                    };
                    if (!avalService.SalvarAvaliacao(aval))
                        sucesso = false;
                }
                if (etRepeticao3.Text != "")
                {
                    var aval = new Avaliacao
                    {
                        IdEstudo = idEstudo,
                        Repeticao = 3,
                        Valor = decimal.Parse(etRepeticao3.Text.Replace(".", ",")),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Data = DateTime.Now,
                        idAvaliacao_Tipo = int.Parse(idTipoAvaliacao)
                    };
                    if (!avalService.SalvarAvaliacao(aval))
                        sucesso = false;
                }
                if (etRepeticao4.Text != "")
                {
                    var aval = new Avaliacao
                    {
                        IdEstudo = idEstudo,
                        Repeticao = 4,
                        Valor = decimal.Parse(etRepeticao4.Text.Replace(".", ",")),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Data = DateTime.Now,
                        idAvaliacao_Tipo = int.Parse(idTipoAvaliacao)
                    };
                    if (!avalService.SalvarAvaliacao(aval))
                        sucesso = false;
                }
                if (etRepeticao5.Text != "")
                {
                    var aval = new Avaliacao
                    {
                        IdEstudo = idEstudo,
                        Repeticao = 5,
                        Valor = decimal.Parse(etRepeticao5.Text.Replace(".", ",")),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Data = DateTime.Now,
                        idAvaliacao_Tipo = int.Parse(idTipoAvaliacao)
                    };
                    if (!avalService.SalvarAvaliacao(aval))
                        sucesso = false;
                }

                if (sucesso)
                {
                    etRepeticao1.Text = etRepeticao2.Text = etRepeticao3.Text = etRepeticao4.Text = etRepeticao5.Text = "";

                    alerta.SetTitle("Sucesso!");
                    alerta.SetIcon(Android.Resource.Drawable.IcDialogInfo);
                    alerta.SetMessage("Avaliação Salva com Sucesso!");
                    alerta.SetButton("OK", (s, ev) =>
                    {
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
                idEstudo = estudo[0].IdEstudo;
                totalRepeticoes = estudo[0].Repeticao;
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

        private void GetAvaliacaoTipo()
        {
            tipos = new ArrayList();
            idTipos = new ArrayList();
            TipoAvaliacaoService tas = new TipoAvaliacaoService();

            var result = tas.GetAvaliacaoTipo();

            foreach (var res in result)
            {
                tipos.Add(res.Descricao);
                idTipos.Add(res.IdAvaliacao_Tipo);
            }

        }

    }
}
