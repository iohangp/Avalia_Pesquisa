using System;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Support.Design.Widget;
using System.Threading;
using Android.Views;
using Android.Content;

namespace Avalia_Pesquisa.Droid
{
    [Activity(Label = "ConfigAparelhoActivity")]
    public class ConfigAparelhoActivity : Activity
    {
        Button saveButton;
        EditText licenca;
        CloudDataStore CloudData;
        DataBase db;
        int statusBarra;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // Create your application here
            SetContentView(Resource.Layout.Config_Aparelho);

            saveButton = FindViewById<Button>(Resource.Id.BTInstalar);
            licenca = FindViewById<EditText>(Resource.Id.EDLicenca);

            saveButton.Click += SaveButton_Click;

            licenca.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    e.Handled = true;
                    saveButton.PerformClick();
                }
            };
        }

        void SaveButton_Click(object sender, EventArgs e)
        {
            CloudData = new CloudDataStore();
            db = new DataBase();

            ProgressDialog pbar = new ProgressDialog(this);
            pbar.SetCancelable(false);
            pbar.SetMessage("Validando chave...");
            pbar.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pbar.Progress = 0;
            pbar.Max = 100;
            pbar.Show();

            bool sucesso = false;
            new Thread(new ThreadStart(async delegate
            {
                if (await CloudData.ValidarChave(licenca.Text))
                    sucesso = true;

                if (sucesso)
                {
                    RunOnUiThread(() => { Toast.MakeText(this, "Chave validada com sucesso.", ToastLength.Long).Show(); });
                    RunOnUiThread(() => { pbar.SetMessage("Sincronizando dados básicos..."); });
                    pbar.Progress += 25;
                    if (await CloudData.MunicipiosSync(licenca.Text) &&
                        await CloudData.LocalidadeSync(licenca.Text))
                    {
                        pbar.Progress += 25;
                        bool result = await CloudData.UsuarioSync(licenca.Text);
                        if (result)
                        {
                            pbar.Progress += 25;
                            var conf2 = new Config
                            {
                                Descricao = "carga_inicial",
                                Valor = "1"
                            };
                            if (db.InserirConfig(conf2))
                                pbar.Progress += 25;

                        }
                    }

                    Thread.Sleep(400);

                    if (pbar.Progress >= 100)
                    {
                        var intent = new Intent(this, typeof(LoginActivity)); ;
                        StartActivity(intent);
                        Finish();
                        pbar.Dismiss();
                    }

                    RunOnUiThread(() => { pbar.SetMessage("Dados importados..."); });
                    RunOnUiThread(() => { Toast.MakeText(this, "Dados importados com sucesso.", ToastLength.Long).Show(); });
                    // SyncInstall();
                }
                else
                {
                    pbar.Dismiss();
                    RunOnUiThread(() => { Toast.MakeText(this, "Chave não encontrada.", ToastLength.Long).Show(); });
                }
              
            })).Start();

          

          //  Finish();
        }

        
    }
}
