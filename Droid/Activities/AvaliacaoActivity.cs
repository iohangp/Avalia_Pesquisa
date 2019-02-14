
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
        Spinner spnTipo, spnAlvo;
        ArrayAdapter adapter;
        ArrayList tipos, idTipos, alvos, idAlvos;
        EditText edNumEstudo, etRepeticao1, etRepeticao2, etRepeticao3, etRepeticao4, etRepeticao5;
        int totalRepeticoes = 1, idEstudo, idPlanejamento, idInstalacao, Tratamento;
        string idTipoAvaliacao, idAlvoSelect;
        TableRow rowRepeticao1,rowRepeticao2,rowRepeticao3,rowRepeticao4,rowRepeticao5,
                 rowAlvo,rowTipoAval,rowPlanejamento;
        Button buttonSalvar;
        TextView textData;

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
            Button buttonValida = FindViewById<Button>(Resource.Id.BTValidar);
            Button buttonScan = FindViewById<Button>(Resource.Id.BTScannerAvalia);

            rowRepeticao1 = FindViewById<TableRow>(Resource.Id.trRepeticao1);
            rowRepeticao2 = FindViewById<TableRow>(Resource.Id.trRepeticao2);
            rowRepeticao3 = FindViewById<TableRow>(Resource.Id.trRepeticao3);
            rowRepeticao4 = FindViewById<TableRow>(Resource.Id.trRepeticao4);
            rowRepeticao5 = FindViewById<TableRow>(Resource.Id.trRepeticao5);

            rowAlvo = FindViewById<TableRow>(Resource.Id.trAlvo);
            rowTipoAval = FindViewById<TableRow>(Resource.Id.trTipoAvaliacao);
            rowPlanejamento = FindViewById<TableRow>(Resource.Id.trDataPlanejada);

            etRepeticao1 = FindViewById<EditText>(Resource.Id.etRepeticao1);
            etRepeticao2 = FindViewById<EditText>(Resource.Id.etRepeticao2);
            etRepeticao3 = FindViewById<EditText>(Resource.Id.etRepeticao3);
            etRepeticao4 = FindViewById<EditText>(Resource.Id.etRepeticao4);
            etRepeticao5 = FindViewById<EditText>(Resource.Id.etRepeticao5);

            buttonScan.Click += BTScanner_Click;
            buttonSalvar.Click += BTSalvar_Click;

            buttonValida.Click += (sender, e) =>
            {          
                   ValidarEstudo(edNumEstudo.Text);
            };

            spnTipo.ItemSelected += SpnTipo_ItemSelected;
            spnAlvo.ItemSelected += SpnAlvo_ItemSelected;


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

            if(int.Parse(idTipoAvaliacao) > 0)
                GetAlvos(int.Parse(idTipoAvaliacao), idEstudo, idPlanejamento);
        }

        private void SpnAlvo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idAlvoSelect = idAlvos[e.Position].ToString();
        }

        protected internal void BTScanner_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(QrCodeActivity));
            StartActivityForResult(intent, 1);
        }

        protected internal void BTSalvar_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alerta = new AlertDialog.Builder(this);

            if (ValidarData(idEstudo))
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
                        sucesso = false;

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
                        sucesso = false;
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
                        sucesso = false;
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
                        AvaliacaoService aval = new AvaliacaoService();
                        var plan = aval.GetDataAvaliacao(idEstudo);

                        if (plan.Count > 0)
                        {
                            idPlanejamento = plan[0].idEstudo_planejamento;
                            textData.Text = plan[0].data.ToString("dd/MM/yyyy");

                            GetAvaliacaoTipo(idEstudo, idPlanejamento);

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

            string[] ids = new string[3];
            bool erroCod = false;

            if (!string.IsNullOrEmpty(protocolo)) {

                if (protocolo.IndexOf('-') != -1)
                    ids = protocolo.Split('-');

                int cont = 0;
                while (cont <= 2)
                {
                    if (ids[cont] == null)
                    {
                        ids[cont] = "0";
                        erroCod = true;
                    }
                    cont++;
                }

                ConsultaEstudoService ces = new ConsultaEstudoService();
                var estudo = ces.GetEstudo(int.Parse(ids[0]));

                idInstalacao = default(int);
                if (estudo.Count > 0 && !erroCod) {

                    idEstudo = estudo[0].IdEstudo;
                    totalRepeticoes = estudo[0].Repeticao;
                    idInstalacao = int.Parse(ids[1]);
                    Tratamento = int.Parse(ids[2]);
                    edNumEstudo.Text = estudo[0].Codigo;
                    AvaliacaoService aval = new AvaliacaoService();
                    var plan = aval.GetDataAvaliacao(idEstudo);

                    if (plan.Count > 0) {

                        idPlanejamento = plan[0].idEstudo_planejamento;
                        textData.Text = plan[0].data.ToString("dd/MM/yyyy"); 

                        GetAvaliacaoTipo(idEstudo, idPlanejamento);
                        rowTipoAval.Visibility = ViewStates.Visible;
                        rowAlvo.Visibility = ViewStates.Visible;
                        rowPlanejamento.Visibility = ViewStates.Visible;

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
            etRepeticao1.Text = etRepeticao2.Text = etRepeticao3.Text = etRepeticao4.Text = etRepeticao5.Text = "";
            rowRepeticao1.Visibility = ViewStates.Invisible;
            rowRepeticao2.Visibility = ViewStates.Invisible;
            rowRepeticao3.Visibility = ViewStates.Invisible;
            rowRepeticao4.Visibility = ViewStates.Invisible;
            rowRepeticao5.Visibility = ViewStates.Invisible;

            rowTipoAval.Visibility = ViewStates.Invisible;
            rowAlvo.Visibility = ViewStates.Invisible;
            rowPlanejamento.Visibility = ViewStates.Invisible;

            buttonSalvar.Visibility = ViewStates.Invisible;
        }

        private bool ValidarData(int idEstudo)
        {
            AvaliacaoService aval = new AvaliacaoService();
            var plan = aval.GetDataAvaliacao(idEstudo);

            if (plan[0].data > DateTime.Now.AddDays(5))
                return true;
            else
                return false;
            
        }

        private void GetAvaliacaoTipo(int idEstudo, int idPlanejamento)
        {
            tipos = new ArrayList();
            idTipos = new ArrayList();
            TipoAvaliacaoService tas = new TipoAvaliacaoService();

            tipos.Add("Selecione");
            idTipos.Add(0);

            var result = tas.GetAvaliacaoTipo(idEstudo, idPlanejamento);

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

        private void GetAlvos(int idTipoAvaliacao, int idEstudo, int idPlanejamento)
        {
            alvos = new ArrayList();
            idAlvos = new ArrayList();
            AvaliacaoService aval = new AvaliacaoService();

            alvos.Add("Selecione");
            idAlvos.Add(0);

            var result = aval.GetAlvos(idTipoAvaliacao,idEstudo,idPlanejamento);

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
