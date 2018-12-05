
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Sincronização")]
    public class SincronizacaoActivity : BaseActivity
    {
        CloudDataStore CloudData;
        DataBase db;

        protected override int LayoutResource => Resource.Layout.Sincronizacao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           

            Button btnSinc = FindViewById<Button>(Resource.Id.BTSincronizar);
            btnSinc.Click += SincronizarClick;
            // Create your application here
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

        protected internal void SincronizarClick(object sender, EventArgs e)
        {
            CloudData = new CloudDataStore();
            db = new DataBase();
            TextView TVResult = FindViewById<TextView>(Resource.Id.TVResultado);

            ProgressDialog pbar = new ProgressDialog(this);
            pbar.SetCancelable(false);
            pbar.SetMessage("Sincronizando dados do sistema...");
            pbar.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pbar.Progress = 0;
            pbar.Max = 100;
            pbar.Show();

            new Thread(new ThreadStart(async delegate
            {

                if (await CloudData.MunicipiosSync(null) &&
                    await CloudData.LocalidadeSync(null))
                {
                    pbar.Progress += 20;
                    if (await CloudData.UsuarioSync(null))
                    {
                        pbar.Progress += 10;
                    }
                }
                if (await CloudData.BaixarCultura(null))
                    pbar.Progress += 10;
                if (await CloudData.BaixarEstudos(null))
                    pbar.Progress += 10;
                if (await CloudData.BaixarVariedade(null) &&
                    await CloudData.BaixarTipoAvaliacao(null))
                    pbar.Progress += 20;
                if (await CloudData.BaixarSafra(null) &&
                    await CloudData.BaixarAlvo(null))
                    pbar.Progress += 10;
                if (await CloudData.BaixarUmidade(null) &&
                    await CloudData.BaixarGleba(null))
                    pbar.Progress += 10;
                if (await CloudData.BaixarSolo(null) &&
                    await CloudData.BaixarCobertura(null))
                    pbar.Progress += 10;



                if (pbar.Progress >= 100)
                {
                    Thread.Sleep(800);
                    RunOnUiThread(() => {
                        TVResult.Text = "Sincronização efetuada com sucesso!";
                        TVResult.SetTextColor(Android.Graphics.Color.DarkGreen);
                    });
                    RunOnUiThread(() => { Toast.MakeText(this, "Dados importados com sucesso.", ToastLength.Long).Show(); });

                    pbar.Dismiss();
                }
                else
                {
                    RunOnUiThread(() => {
                        TVResult.Text = "Erro ao baixar os dados do servidor!";
                        TVResult.SetTextColor(Android.Graphics.Color.Red);
                    });

                    pbar.Dismiss();

                }
                
                
                RunOnUiThread(() => { pbar.SetMessage("Dados importados..."); });
             
            })).Start();
        }
    }
}
