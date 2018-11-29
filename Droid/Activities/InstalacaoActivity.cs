
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
       //private EventHandler<AdapterView.ItemSelectedEventArgs> spnPlantio_ItemSelected;

        protected override int LayoutResource => Resource.Layout.Instalacao;




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Instalacao);
            // Create your application here

            spnPlantio = FindViewById<Spinner>(Resource.Id.spnPlantio);
            edNumEstudo = FindViewById<EditText>(Resource.Id.EDNumEstudo);
            buttonSalvar = FindViewById<Button>(Resource.Id.BTSalvarAvaliacao);
            Button buttonValida = FindViewById<Button>(Resource.Id.BTValidar);
            Button buttonScan = FindViewById<Button>(Resource.Id.BTScannerAvalia);

            spnPlantio.ItemSelected += spnPlantio_ItemSelected;

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

            spnPlantio.ItemSelected += spnPlantio_ItemSelected;

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


        private void spnPlantio_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idPlantio = idPlantio[e.Position].ToString();

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


        protected internal void BTSalvar_Click(object sender, EventArgs e)
        {

            etComprimento = FindViewById<EditText>(Resource.Id.etComprimento);
            etLargura = FindViewById<EditText>(Resource.Id.etLargura);
            etCoordenadas1 = FindViewById<EditText>(Resource.Id.etCoordenadas1);
            etCoordenadas2 = FindViewById<EditText>(Resource.Id.etCoordenadas2);
            etAltitude = FindViewById<EditText>(Resource.Id.etAltitude);
            etObservacoes = FindViewById<EditText>(Resource.Id.etObservacoes);
            etData = FindViewById<EditText>(Resource.Id.etData);


            InstalacaoService avalService = new InstalacaoService();

            var aval = new Instalacao
            {

                idEstudo = 1,
                idPlantio = 1,
                Tamanho_Parcela_Comprimento = 10,
                Tamanho_Parcela_Largura = 11,
                Coordenadas1 = "12",
                Coordenadas2 = "13",
                Altitude = "40",
                Data_Instalacao = DateTime.Now,
                idUsuario = 1,
                Observacoes = "deu certo"

             };
        


            avalService.SalvarInstalacao(aval);
                

        }

        private void ValidarEstudo(string protocolo)
        {

            ConsultaEstudoService ces = new ConsultaEstudoService();
            var estudo = ces.GetEstudo(protocolo);
            buttonSalvar.Visibility = ViewStates.Visible;

        }




        private void GetPlantio()
        {
            Plantio = new ArrayList();
            idPlantios = new ArrayList();
            PlantacaoService tas = new PlantacaoService();


            var result = tas.GetPlantio();

          //  if (result.Count > 0)
//{
                foreach (var res in result)
                {
                    Plantio.Add(res.Descricao);
                    idPlantios.Add(res.idPlantio);
                }
          //  }

        }


    }
}
