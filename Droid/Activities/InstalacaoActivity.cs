
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
using Plugin.ExternalMaps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "InstalacaoActivity")]
    public class InstalacaoActivity : BaseActivity
    {

        Spinner spnPlantio;
        ArrayAdapter adapter;
        ArrayList Plantio, idPlantios;
        EditText edNumEstudo, etComprimento, etLargura, etCoordenadas1, etCoordenadas2, etAltitude, etObservacoes, etData;
        // int totalRepeticoes = 1, idEstudo;
        string idPlantio, idCultura;
        // TableRow rowRepeticao1, rowRepeticao2, rowRepeticao3, rowRepeticao4, rowRepeticao5;
        Button buttonSalvar;
        int idEstudo_;
        double latitude = 0;
        double longitude = 0;
        double altitude = 0;
        //private EventHandler<AdapterView.ItemSelectedEventArgs> spnPlantio_ItemSelected;

        protected override int LayoutResource => Resource.Layout.Instalacao;




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Instalacao);
            // Create your application here

            spnPlantio = FindViewById<Spinner>(Resource.Id.spnPlantio);
            edNumEstudo = FindViewById<EditText>(Resource.Id.EDNumEstudo);
            buttonSalvar = FindViewById<Button>(Resource.Id.BTSalvarInstalacao);
            Button buttonValida = FindViewById<Button>(Resource.Id.BTValidar);
            Button buttonScan = FindViewById<Button>(Resource.Id.BTScannerAvalia);

            etComprimento = FindViewById<EditText>(Resource.Id.etComprimento);
            etLargura = FindViewById<EditText>(Resource.Id.etLargura);
            etCoordenadas1 = FindViewById<EditText>(Resource.Id.etCoordenadas1);
            etCoordenadas2 = FindViewById<EditText>(Resource.Id.etCoordenadas2);
            etAltitude = FindViewById<EditText>(Resource.Id.etAltitude);
            etObservacoes = FindViewById<EditText>(Resource.Id.etObservacoes);
            etData = FindViewById<EditText>(Resource.Id.etData);

           

            GetPlantio();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Plantio);
            //vincula o adaptador ao controle spinner
            spnPlantio.Adapter = adapter;

            buttonScan.Click += BTScanner_Click;
            buttonSalvar.Click += BTSalvar_Click;

            buttonValida.Click += (sender, e) =>
            {
                ValidarEstudo(edNumEstudo.Text);


            };

            spnPlantio.ItemSelected += SpnPlantio_ItemSelected;

            Coordenadas();

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


        private void SpnPlantio_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idPlantio = idPlantios[e.Position].ToString();

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


        public void BTSalvar_Click(object sender, EventArgs e)
        {
            InstalacaoService avalService = new InstalacaoService();

            var aval = new Instalacao
            {

                idEstudo = idEstudo_,
                idPlantio = 1,
                Tamanho_Parcela_Comprimento = decimal.Parse(etComprimento.Text.Replace(".", ",")),
                Tamanho_Parcela_Largura = decimal.Parse(etLargura.Text.Replace(".", ",")),
                Coordenadas1 = etCoordenadas1.Text,
                Coordenadas2 = etCoordenadas2.Text,
                Altitude = etAltitude.Text,
                Data_Instalacao = DateTime.Now,
                idUsuario = int.Parse(Settings.GeneralSettings),
                Observacoes = etObservacoes.Text

            };

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();

            try
            {
                avalService.SalvarInstalacao(aval);
               

                alerta.SetTitle("Sucesso!");
                alerta.SetIcon(Android.Resource.Drawable.IcInputAdd);
                alerta.SetMessage("Instalação Salva com Sucesso!");
                alerta.SetButton("OK", (s, ev) =>
                {
                    alerta.Dismiss();
                });
                alerta.Show();
            }

            catch

            {
                alerta.SetMessage("Erro ao salvar ");
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


        private void LimpaCampos()
        {
            etComprimento.Text = etData.Text = etLargura.Text = etAltitude.Text = etObservacoes.Text = etCoordenadas1.Text = etCoordenadas2.Text = "";

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


        private void GetPlantio()
        {
            Plantio = new ArrayList();
            idPlantios = new ArrayList();
            PlantacaoService tas = new PlantacaoService();

            Plantio.Add("Selecione");
            idPlantios.Add(0);

            var result = tas.GetPlantio();

            if (result.Count > 0)
            {
                foreach (var res in result)
                {
                    Plantio.Add(res.Observacoes);
                    idPlantios.Add(res.idPlantio);
                }
            }

        }

        public async void Coordenadas()
        {

            var locator = CrossGeolocator.Current;

            Position position = null;
            try
            {
                    locator.DesiredAccuracy = 100;
                    //etObservacoes.Text += "Status: " + position.Timestamp + "\n";
                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);
                    longitude = position.Longitude;
                    latitude = position.Latitude;
                    altitude = position.Altitude;
                    etCoordenadas1.Text = latitude.ToString();
                    etCoordenadas2.Text = longitude.ToString();
                    etAltitude.Text = altitude.ToString();

            }
        
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine(ex.Message);
            }
        }
    }

}