
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
    [Activity(Label = "Instalação")]
    public class InstalacaoActivity : BaseActivity
    {

        Spinner spnLocalidade, spnGlebaCult;
        ArrayAdapter adapter;
        ArrayList Localidades, idLocalidades, GlebaCults, idGlebaCults;
        EditText edNumEstudo, etComprimento, etLargura, etCoordenadas1, etCoordenadas2, etAltitude, etObservacoes;
        // int totalRepeticoes = 1, idEstudo;
        string idPlantio, idCultura, idLocalidadeSelect, idPlantioSelect;
        // TableRow rowRepeticao1, rowRepeticao2, rowRepeticao3, rowRepeticao4, rowRepeticao5;
        Button buttonSalvar;
        int idEstudo_;
        double latitude = 0;
        double longitude = 0;
        double altitude = 0;
        EditText textDate;
        ImageButton buttonCalendar;


        //private EventHandler<AdapterView.ItemSelectedEventArgs> spnPlantio_ItemSelected;

        protected override int LayoutResource => Resource.Layout.Instalacao;




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //    SetContentView(Resource.Layout.Instalacao);
            // Create your application here

            spnLocalidade = FindViewById<Spinner>(Resource.Id.spnLocalidade);
            spnGlebaCult = FindViewById<Spinner>(Resource.Id.spnGlebaCult);
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
            //etData = FindViewById<EditText>(Resource.Id.etData);

            textDate = FindViewById<EditText>(Resource.Id.etData);
            textDate.AddTextChangedListener(new Mask(textDate, "##/##/####"));
            buttonCalendar = FindViewById<ImageButton>(Resource.Id.ibData);



            GetLocPlantio();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Localidades);
            //vincula o adaptador ao controle spinner
            spnLocalidade.Adapter = adapter;

            GlebaCults = new ArrayList();
            idGlebaCults = new ArrayList();
            GlebaCults.Add("Selecione");
            idGlebaCults.Add(0);

            buttonCalendar.Click += DateSelect_OnClick;

            buttonScan.Click += BTScanner_Click;
            buttonSalvar.Click += BTSalvar_Click;

            buttonValida.Click += (sender, e) =>
            {
                ValidarEstudo(edNumEstudo.Text);


            };

            spnLocalidade.ItemSelected += SpnLocalidade_ItemSelected;
            spnGlebaCult.ItemSelected += SpnGlebaCult_ItemSelected;

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


        private void SpnLocalidade_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idLocalidadeSelect = idLocalidades[e.Position].ToString();

            GetPlantio(int.Parse(idLocalidadeSelect));

            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, GlebaCults);
            spnGlebaCult.Adapter = adapter;

        }

        private void SpnGlebaCult_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idPlantioSelect = idGlebaCults[e.Position].ToString();

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
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();

            if (idEstudo_ > 0)
            {
                if ((etComprimento.Text != "") && (etComprimento.Text != "") && (idPlantioSelect!="0"))
                {
                    var date = "";
                    if(textDate.Text =="")
                        date = DateTime.Now.ToString();
                    else {
                        date = textDate.Text;
                    }


                    var aval = new Instalacao
                    {

                        idEstudo = idEstudo_,
                        idPlantio = int.Parse(idPlantioSelect),
                        Tamanho_Parcela_Comprimento = decimal.Parse(etComprimento.Text.Replace(".", ",")),
                        Tamanho_Parcela_Largura = decimal.Parse(etLargura.Text.Replace(".", ",")),
                        Coordenadas1 = etCoordenadas1.Text,
                        Coordenadas2 = etCoordenadas2.Text,
                        Altitude = etAltitude.Text,
                        Data_Instalacao = Convert.ToDateTime(date),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        Observacoes = etObservacoes.Text

                    };

                    try
                    {
                        if (avalService.SalvarInstalacao(aval) == true)
                        {

                            alerta.SetTitle("Sucesso!");
                            alerta.SetIcon(Android.Resource.Drawable.IcInputAdd);
                            alerta.SetMessage("Instalação Salva com Sucesso!");
                            alerta.SetButton("OK", (s, ev) =>
                            {
                                alerta.Dismiss();
                            });
                            alerta.Show();
                            LimpaCampos();
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
                        
                    catch

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
                else {
                    alerta.SetMessage("Favor preencher todos os campos obrigatórios");
                    alerta.SetTitle("ERRO!");
                    alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    alerta.SetMessage("Favor preencher os campos obrigatórios!");
                    alerta.SetButton("OK", (s, ev) =>
                    {
                        alerta.Dismiss();
                    });
                    alerta.Show();
                }
            }

            else {
                alerta.SetMessage("Favor informar um estudo válido ");
                alerta.SetTitle("ERRO!");
                alerta.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                alerta.SetMessage("Favor informar um estudo válido!");
                alerta.SetButton("OK", (s, ev) =>
                {
                    alerta.Dismiss();
                });
                alerta.Show();
            }

        }


        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time) {
                textDate.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }


        private void LimpaCampos()
        {
            etComprimento.Text = textDate.Text = etLargura.Text = etAltitude.Text = etObservacoes.Text = etCoordenadas1.Text = etCoordenadas2.Text = "";
        }

        private void ValidarEstudo(string protocolo)
        {
            string[] ids = new string[2]; ;

            if (protocolo.IndexOf('|') != -1)
                ids = protocolo.Split('|');
            else
                ids[0] = protocolo;

            ConsultaEstudoService ces = new ConsultaEstudoService();
            var estudo = ces.GetEstudo(ids[0]);
            buttonSalvar.Visibility = ViewStates.Visible;

            if (estudo.Count > 0)
            {
                idEstudo_ = estudo[0].IdEstudo;
                edNumEstudo.Text = estudo[0].Codigo;
                
            }
            else
            {
                Toast.MakeText(this, "Nenhum estudo encontrado", ToastLength.Long).Show();
            }


        }


        private void GetLocPlantio()
        {
            Localidades = new ArrayList();
            idLocalidades = new ArrayList();
            PlantacaoService tas = new PlantacaoService();

            Localidades.Add("Selecione");
            idLocalidades.Add(0);

            var result = tas.GetPlantioLocalidade();

            if (result.Count > 0)
            {
                foreach (var res in result)
                {
                    Localidades.Add(res.Descricao);
                    idLocalidades.Add(res.IdLocalidade);
                }
            }

        }

        private void GetPlantio(int idLocalidade)
        {
            GlebaCults = new ArrayList();
            idGlebaCults = new ArrayList();
            GlebaCults.Add("Selecione");
            idGlebaCults.Add(0);
            PlantacaoService tas = new PlantacaoService();

            var result = tas.GetPlantioGlebaCult(idLocalidade);

            if (result.Count > 0)
            {
                foreach (var res in result)
                {
                    GlebaCults.Add(res.Gleba+" - "+res.Cultura);
                    idGlebaCults.Add(res.idPlantio);
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

        public class DatePickerFragment : DialogFragment,
        DatePickerDialog.IOnDateSetListener
        {
            // TAG can be any string of your choice.  
            public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();
            // Initialize this value to prevent NullReferenceExceptions.  
            Action<DateTime> _dateSelectedHandler = delegate { };
            public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
            {
                DatePickerFragment frag = new DatePickerFragment();
                frag._dateSelectedHandler = onDateSelected;
                return frag;
            }
            public override Dialog OnCreateDialog(Bundle savedInstanceState)
            {
                DateTime currently = DateTime.Now;
                DatePickerDialog dialog = new DatePickerDialog(Activity, this, currently.Year, currently.Month - 1, currently.Day);
                return dialog;
            }
            public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
            {
                // Note: monthOfYear is a value between 0 and 11, not 1 and 12!  
                DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
                Android.Util.Log.Debug(TAG, selectedDate.ToLongDateString());
                _dateSelectedHandler(selectedDate);
            }
        }

    }

} 

