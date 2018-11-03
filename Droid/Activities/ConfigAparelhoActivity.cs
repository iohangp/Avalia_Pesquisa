﻿using System;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Support.Design.Widget;
using System.Threading;

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
        }

        void SaveButton_Click(object sender, EventArgs e)
        {
            CloudData = new CloudDataStore();
            db = new DataBase();

            ProgressDialog pbar = new ProgressDialog(this);
            pbar.SetCancelable(true);
            pbar.SetMessage("Sincronizando dados do sistema...");
            pbar.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pbar.Progress = 0;
            pbar.Max = 100;
            pbar.Show();

            new Thread(new ThreadStart(delegate
            {
                var conf = new Config
                {
                    Descricao = "chave_aparelho",
                    Valor = licenca.Text
                };
                    
                if (db.InserirConfig(conf))
                {
                    pbar.Progress += 30;
                    if (CloudData.MunicipiosSync(licenca.Text))
                    {
                        pbar.Progress += 30;
                        if(CloudData.UsuarioSync(licenca.Text))
                            pbar.Progress += 40;
                    }

                }

                Thread.Sleep(400);

                if(pbar.Progress >= 100)
                    Finish();


                RunOnUiThread(() => { pbar.SetMessage("Dados importados..."); });
                RunOnUiThread(() => { Toast.MakeText(this, "Dados importados com sucesso.", ToastLength.Long).Show(); });
            })).Start();

          //  Finish();
        }
    }
}
