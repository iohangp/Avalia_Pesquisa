
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
    [Activity(Label = "Plantação")]
    public class PlantacaoActivity : BaseActivity
    {
        ArrayList culturas, idCulturas, variedade, idVariedade, glebas, idGleba, safras, idSafras,
                  umidades, idUmidades, culturaAnt, idCulturaAnt;
        ArrayAdapter adapter, adapterVar;
        Spinner spinnerCult, spinnerVar, spinnerGleba, spinnerSafra, spinnerUmidade, spinnerCultAnt;
        string idCulturaSelect, idVarSelect;

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

            variedade = new ArrayList();
            idVariedade = new ArrayList();
            variedade.Add("Selecione");
            idVariedade.Add(0);

            GetCulturas();
            GetGlebas();
            GetSafras();
            GetUmidade();

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


            spinnerCult.ItemSelected += SpinnerCult_ItemSelected;
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

        private void SpinnerCult_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idCulturaSelect = idCulturas[e.Position].ToString();
            GetVariedade(int.Parse(idCulturaSelect));

            adapterVar = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, variedade);

            spinnerVar.Adapter = adapterVar;
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
    }
}
