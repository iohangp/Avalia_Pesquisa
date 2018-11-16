
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
using Android.Text;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Consultar Estudo")]
    public class ConsultaEstudo : BaseActivity
    {

        ViewPager pager;
        TabsAdapter adapter;
        DataBase db;
        CloudDataStore CloudData;
        TextView txtEstudo, txtProtocolo, txtCultura, txtPatrocinador, txtProduto;

        protected override int LayoutResource => Resource.Layout.ConsultaEstudo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           // SetContentView(Resource.Layout.ConsultaEstudo);
            txtEstudo = FindViewById<TextView>(Resource.Id.TVNumeroEstudo);
            Button buttonConsultar2 = FindViewById<Button>(Resource.Id.BTConsultar2);
            Button buttonScanner = FindViewById<Button>(Resource.Id.BTScanner);
          
            // Create your application here
            
            buttonConsultar2.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AplicacaoActivity)); ;
                StartActivity(intent);
            };

            buttonScanner.Click += BTScanner_Click;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            txtEstudo = FindViewById<TextView>(Resource.Id.TVNumeroEstudo);
            
            if (requestCode == 1)
            {
                if (resultCode == Result.Ok)
                {
                    if (data.GetStringExtra("qrcode") != null)
                    {
                        txtEstudo.Text = data.GetStringExtra("qrcode");
                        ConsultarEstudo(txtEstudo.Text);
                        
                    }
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        public void ConsultarEstudo(string protocolo)
        {
            txtProtocolo = FindViewById<TextView>(Resource.Id.TVProtocolo);
            txtPatrocinador = FindViewById<TextView>(Resource.Id.TVPatrocinador);
            txtProduto = FindViewById<TextView>(Resource.Id.TVProduto);

            ConsultaEstudoService ces = new ConsultaEstudoService();
            var estudo = ces.GetEstudo(txtEstudo.Text);

            if (estudo.Count > 0)
            {
                txtProtocolo.Text = estudo[0].Protocolo;
                txtPatrocinador.Text = estudo[0].Cliente;
                txtProduto.Text = estudo[0].Produto;
            }
            else
            {
                Toast.MakeText(this, "Nenhum estudo encontrado", ToastLength.Long).Show();
            }

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

        public override void OnBackPressed()
        {
            Finish();
        }

        protected internal void BTScanner_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(QrCodeActivity));
            StartActivityForResult(intent,1);
        }

    }
}
