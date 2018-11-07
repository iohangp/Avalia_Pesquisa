
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
using Avalia_Pesquisa.Droid.Activities;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using System.Threading;
using Avalia_Pesquisa.Droid.Helpers;
using Android.Content.PM;
using static Android.Support.V7.Widget.RecyclerView;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "ConsultaEstudo")]
    public class ConsultaEstudo : Activity
    {

        ViewPager pager;
        TabsAdapter adapter;
        DataBase db;
        CloudDataStore CloudData;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ConsultaEstudo);
            CriarBancoDados();
            // Create your application here

            Button buttonConsultar2 = FindViewById<Button>(Resource.Id.BTConsultar2);
            buttonConsultar2.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AplicacaoActivity)); ;
                StartActivity(intent);
            };
        }

        private void CriarBancoDados()
        {
            db = new DataBase();
            db.CriarBancoDeDados();
        }

        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(MainActivity)); ;
            StartActivity(intent);

            return;
        }

        protected internal void BTScanner_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(QrCodeActivity)); ;
            StartActivity(intent);
        }

}
