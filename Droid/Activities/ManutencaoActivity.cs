
using System;
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
    [Activity(Label = "ManutencaoActivity")]
    public class ManutencaoActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Manutencao);
            // Create your application here
        }

        //Botao Voltar do celular
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(MainActivity)); ;
            StartActivity(intent);

            return;
        }
    }
}
