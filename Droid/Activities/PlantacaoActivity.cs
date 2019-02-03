
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Avalia_Pesquisa.Droid.Helpers;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Plantação")]
    public class PlantacaoActivity : BaseActivity
    {
        ArrayList culturas, idCulturas, variedade, idVariedade, glebas, idGleba, safras, idSafras,
                  umidades, idUmidades, culturaAnt, idCulturaAnt, solos, idSolos, coberturas, idCoberturas,
                  status, idStatus, localidades, idLocalidades;
        ArrayAdapter adapter, adapterVar;
        Spinner spinnerCult, spinnerVar, spinnerGleba, spinnerSafra, spinnerUmidade,
                spinnerCultAnt, spinnerSolo, spinnerCobertura, spinnerStatus, spinnerLocalidade;
        string idCulturaSelect, idVarSelect, idLocSelect, idSafraSelect, idGlebaSelect, idUmiSelect,
               idCultAntSelect, idSoloSelect, idCobSelect, idStatusSelect;
        TextView textDate, textDateGerm;
        ImageButton buttonCalendar, buttonDataGerm;
        EditText textAdubaBase, textAdubaCob, textEspacamento, textPopulacao, textObs, textMetragem;
        Button buttonSalvar;

        protected override int LayoutResource => Resource.Layout.Plantio;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.Plantio);
            // Create your application here
            spinnerCult = FindViewById<Spinner>(Resource.Id.SPNCultura);
            spinnerVar = FindViewById<Spinner>(Resource.Id.SPNVariedade);
            spinnerGleba = FindViewById<Spinner>(Resource.Id.SPNGleba);
            spinnerSafra = FindViewById<Spinner>(Resource.Id.SPNSafra);
            spinnerUmidade = FindViewById<Spinner>(Resource.Id.SPNUmidade);
            spinnerCultAnt = FindViewById<Spinner>(Resource.Id.SPNCultAnt);
            spinnerSolo = FindViewById<Spinner>(Resource.Id.SPNSolo);
            spinnerCobertura = FindViewById<Spinner>(Resource.Id.SPNCobertura);
            spinnerStatus = FindViewById<Spinner>(Resource.Id.SPNStatus);
            spinnerLocalidade = FindViewById<Spinner>(Resource.Id.SPNLocalidadePlantio);

            textDate = FindViewById<TextView>(Resource.Id.TVDataPlantio);
            buttonCalendar = FindViewById<ImageButton>(Resource.Id.IBCalendar);
            textDateGerm = FindViewById<TextView>(Resource.Id.TVDataGerm);
            buttonDataGerm = FindViewById<ImageButton>(Resource.Id.IBGerminacao);
            buttonSalvar = FindViewById<Button>(Resource.Id.BTSalvarPlant);

            textAdubaBase = FindViewById<EditText>(Resource.Id.EDAdubaBase);
            textAdubaCob = FindViewById<EditText>(Resource.Id.EDAdubaCob);
            textEspacamento = FindViewById<EditText>(Resource.Id.EDEspacamento);
            textPopulacao = FindViewById<EditText>(Resource.Id.EDPopulacao);
            textObs = FindViewById<EditText>(Resource.Id.EDObservacao);
            textMetragem = FindViewById<EditText>(Resource.Id.EDMetragem);

            buttonCalendar.Click += DateSelect_OnClick;
            buttonDataGerm.Click += GermSelect_OnClick;

            variedade = new ArrayList();
            idVariedade = new ArrayList();
            variedade.Add("Selecione");
            idVariedade.Add(0);

            GetCulturas();
            GetGlebas();
            GetSafras();
            GetUmidade();
            GetSolos();
            GetCoberturas();
            GetLocalidades();

            status = new ArrayList();
            idStatus = new ArrayList();
            status.Add("Ativo");
            idStatus.Add(1);
            status.Add("Inativo");
            idStatus.Add(0);

            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, culturas);
            spinnerCult.Adapter = adapter;
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, glebas);
            spinnerGleba.Adapter = adapter;
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, safras);
            spinnerSafra.Adapter = adapter;
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, umidades);
            spinnerUmidade.Adapter = adapter;
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, culturaAnt);
            spinnerCultAnt.Adapter = adapter;
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, solos);
            spinnerSolo.Adapter = adapter;
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, coberturas);
            spinnerCobertura.Adapter = adapter;
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, status);
            spinnerStatus.Adapter = adapter;
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, localidades);
            spinnerLocalidade.Adapter = adapter;

            spinnerCult.ItemSelected += SpinnerCult_ItemSelected;
            spinnerLocalidade.ItemSelected += SpinnerLoc_ItemSelected;
            spinnerSafra.ItemSelected += SpinnerSafra_ItemSelected;
            spinnerVar.ItemSelected += SpinnerVar_ItemSelected;
            spinnerGleba.ItemSelected += SpinnerGleba_ItemSelected;
            spinnerUmidade.ItemSelected += SpinnerUmidade_ItemSelected;
            spinnerCultAnt.ItemSelected += SpinnerCultAnt_ItemSelected;
            spinnerSolo.ItemSelected += SpinnerTipoSolo_ItemSelected;
            spinnerCobertura.ItemSelected += SpinnerCoberturaSolo_ItemSelected;
            spinnerStatus.ItemSelected += SpinnerStatus_ItemSelected;

            buttonSalvar.Click += BTSalvar_Click;
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

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time) {
                textDate.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        void GermSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time) {
                textDateGerm.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }


        private void SpinnerCult_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idCulturaSelect = idCulturas[e.Position].ToString();
            GetVariedade(int.Parse(idCulturaSelect));

            adapterVar = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, variedade);

            spinnerVar.Adapter = adapterVar;
        }

        private void SpinnerVar_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idVarSelect = idVariedade[e.Position].ToString();
        }

        private void SpinnerLoc_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idLocSelect = idLocalidades[e.Position].ToString();
        }

        private void SpinnerSafra_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idSafraSelect = idSafras[e.Position].ToString();
        }

        private void SpinnerGleba_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idGlebaSelect = idGleba[e.Position].ToString();
        }

        private void SpinnerUmidade_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idUmiSelect = idUmidades[e.Position].ToString();
        }

        private void SpinnerCultAnt_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idCultAntSelect = idCulturaAnt[e.Position].ToString();
        }

        private void SpinnerTipoSolo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idSoloSelect = idSolos[e.Position].ToString();
        }

        private void SpinnerCoberturaSolo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idCobSelect = idCoberturas[e.Position].ToString();
        }

        private void SpinnerStatus_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idStatusSelect = idStatus[e.Position].ToString();
        }

        protected internal void BTSalvar_Click(object sender, EventArgs e)
        {

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alerta = builder.Create();

            if (int.Parse(idCulturaSelect) > 0 && int.Parse(idVarSelect) > 0 && int.Parse(idLocSelect) > 0 &&
                int.Parse(idSafraSelect) > 0 && int.Parse(idGlebaSelect) > 0 && int.Parse(idUmiSelect) > 0 &&
                int.Parse(idCultAntSelect) > 0 && int.Parse(idSoloSelect) > 0 && int.Parse(idCobSelect) > 0)
            {

                try
                {
                    var plan = new Plantio
                    {
                        idCultura = int.Parse(idCulturaSelect),
                        idVariedade = int.Parse(idVarSelect),
                        Data_Plantio = Convert.ToDateTime(textDate.Text),
                        idLocalidade = int.Parse(idLocSelect),
                        idSafra = int.Parse(idSafraSelect),
                        Data_Germinacao = Convert.ToDateTime(textDateGerm.Text),
                        idGleba = int.Parse(idGlebaSelect),
                        idUmidade_Solo = int.Parse(idUmiSelect),
                        Adubacao_Base = decimal.Parse(textAdubaBase.Text),
                        Adubacao_Cobertura = decimal.Parse(textAdubaCob.Text),
                        Espacamento = int.Parse(textEspacamento.Text),
                        Populacao = int.Parse(textPopulacao.Text),
                        Observacoes = textObs.Text,
                        idCulturaAnterior = int.Parse(idCultAntSelect),
                        idSolo = int.Parse(idSoloSelect),
                        idUsuario = int.Parse(Settings.GeneralSettings),
                        idCultura_Cobertura_Solo = int.Parse(idCobSelect),
                        Metragem = decimal.Parse(textMetragem.Text),
                        Status = int.Parse(idStatusSelect),
                        Integrado = 0
                    };

                    PlantacaoService ps = new PlantacaoService();

                    if (ps.SalvarPlantio(plan))
                    {
                        LimparCampos();

                        alerta.SetTitle("Sucesso!");
                        alerta.SetIcon(Android.Resource.Drawable.IcInputAdd);
                        alerta.SetMessage("Plantação salva com sucesso!");
                        alerta.SetButton("OK", (s, ev) =>
                        {
                            alerta.Dismiss();
                        });
                        alerta.Show();
                    }
                    else
                    {
                        alerta.SetTitle("ERRO!");
                        alerta.SetIcon(Android.Resource.Drawable.IcDelete);
                        alerta.SetMessage("Erro ao salvar a plantação. Contate o suporte");
                        alerta.SetButton("OK", (s, ev) =>
                        {
                            alerta.Dismiss();
                        });
                        alerta.Show();
                    }
                    
                }
                catch (Exception ex)
                {
                    // Unable to get location
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {

                alerta.SetTitle("Atenção!");
                alerta.SetIcon(Android.Resource.Drawable.IcDelete);
                alerta.SetMessage("Os campos com * são obrigatórios. Preencha-os e tente novamente.");
                alerta.SetButton("OK", (s, ev) =>
                {
                    alerta.Dismiss();
                });
                alerta.Show();
            }

            

        }

        private void LimparCampos()
        {
            textAdubaBase.Text = textAdubaCob.Text = textEspacamento.Text = textPopulacao.Text = textObs.Text = "";
            textDate.Text = textDateGerm.Text = textMetragem.Text = "";
            idCulturaSelect = idVarSelect = idSafraSelect = idLocSelect = idGlebaSelect = idUmiSelect = "0";
            idCultAntSelect = idSoloSelect = idCobSelect = idStatusSelect = "0";

            spinnerCult.SetSelection(0);
            spinnerLocalidade.SetSelection(0);
            spinnerSafra.SetSelection(0);
            spinnerVar.SetSelection(0);
            spinnerGleba.SetSelection(0);
            spinnerUmidade.SetSelection(0);
            spinnerCultAnt.SetSelection(0);
            spinnerSolo.SetSelection(0);
            spinnerCobertura.SetSelection(0);
            spinnerStatus.SetSelection(0);
        }

        private void GetCulturas()
        {
            culturas = new ArrayList();
            idCulturas = new ArrayList();

            culturaAnt = new ArrayList();
            idCulturaAnt = new ArrayList();
            PlantacaoService tas = new PlantacaoService();

            var result = tas.GetCulturas();

            culturas.Add("Selecione");
            idCulturas.Add(0);
            culturaAnt.Add("Selecione");
            idCulturaAnt.Add(0);
            foreach (var res in result)
            {
                culturas.Add(res.Descricao);
                culturaAnt.Add(res.Descricao);

                idCulturas.Add(res.IdCultura);
                idCulturaAnt.Add(res.IdCultura);
            }

        }

        private void GetVariedade(int idcultura)
        {
            variedade = new ArrayList();
            idVariedade = new ArrayList();
            PlantacaoService tas = new PlantacaoService();

            var result = tas.GetVariedades(idcultura);

            foreach (var res in result)
            {
                variedade.Add(res.Descricao);
                idVariedade.Add(res.IdVariedade);
            }

        }

        private void GetGlebas()
        {
            glebas = new ArrayList();
            idGleba = new ArrayList();
            PlantacaoService tas = new PlantacaoService();

            var result = tas.GetGlebas();

            glebas.Add("Selecione");
            idGleba.Add(0);
            foreach (var res in result)
            {
                glebas.Add(res.Descricao);
                idGleba.Add(res.idGleba);
            }

        }

        private void GetSafras()
        {
            safras = new ArrayList();
            idSafras = new ArrayList();
            PlantacaoService tas = new PlantacaoService();

            var result = tas.GetSafras();

            safras.Add("Selecione");
            idSafras.Add(0);
            foreach (var res in result)
            {
                safras.Add(res.Descricao);
                idSafras.Add(res.IdSafra);
            }

        }

        private void GetUmidade()
        {
            umidades = new ArrayList();
            idUmidades = new ArrayList();
            PlantacaoService tas = new PlantacaoService();

            var result = tas.GetUmidades();

            umidades.Add("Selecione");
            idUmidades.Add(0);
            foreach (var res in result)
            {
                umidades.Add(res.Descricao);
                idUmidades.Add(res.idUmidade_Solo);
            }

        }

        private void GetSolos()
        {
            solos = new ArrayList();
            idSolos = new ArrayList();
            PlantacaoService tas = new PlantacaoService();

            var result = tas.GetSolos();

            solos.Add("Selecione");
            idSolos.Add(0);
            foreach (var res in result)
            {
                solos.Add(res.Descricao);
                idSolos.Add(res.idSolo);
            }

        }

        private void GetCoberturas()
        {
            coberturas = new ArrayList();
            idCoberturas = new ArrayList();
            PlantacaoService tas = new PlantacaoService();

            var result = tas.GetCoberturas();

            coberturas.Add("Selecione");
            idCoberturas.Add(0);
            foreach (var res in result)
            {
                coberturas.Add(res.Descricao);
                idCoberturas.Add(res.idCobertura_Solo);
            }

        }

        private void GetLocalidades()
        {
            localidades = new ArrayList();
            idLocalidades = new ArrayList();
            PlantacaoService tas = new PlantacaoService();

            var result = tas.GetLocalidades();

            localidades.Add("Selecione");
            idLocalidades.Add(0);
            foreach (var res in result)
            {
                localidades.Add(res.Descricao);
                idLocalidades.Add(res.IdLocalidade);
            }

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
            DatePickerDialog dialog = new DatePickerDialog(Activity, this, currently.Year, currently.Month, currently.Day);
            return dialog;
        }
        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!  
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            _dateSelectedHandler(selectedDate);
        }
    }
}
