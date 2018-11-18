
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
        ArrayList culturas, idCulturas, variedade, idVariedade;
        ArrayAdapter adapter, adapterVar;
        Spinner spinnerCult, spinnerVar;
        string idCulturaSelect, idVarSelect;

        protected override int LayoutResource => Resource.Layout.Plantio;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.Plantio);
            // Create your application here
            spinnerCult = FindViewById<Spinner>(Resource.Id.SPNCultura);
            spinnerVar = FindViewById<Spinner>(Resource.Id.SPNVariedade);
            GetCulturas();

            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, culturas);
            //vincula o adaptador ao controle spinner
            spinnerCult.Adapter = adapter;
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
            PlantacaoService tas = new PlantacaoService();

            var result = tas.GetCulturas();

            culturas.Add("Selecione");
            idCulturas.Add(0);
            foreach (var res in result)
            {
                culturas.Add(res.Descricao);
                idCulturas.Add(res.IdCultura);
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
    }
}
