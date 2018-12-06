
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
using Avalia_Pesquisa.Models;
using Avalia_Pesquisa.Services;

namespace Avalia_Pesquisa.Droid.Activities
{
    [Activity(Label = "Dados Meterologicos")]
    public class WeatherActivity : BaseActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Weather);

            Button button = FindViewById<Button>(Resource.Id.weatherBtn);

            button.Click += Button_Click;
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            EditText zipCodeEntry = FindViewById<EditText>(Resource.Id.ZipCodeEntry);

            if (!String.IsNullOrEmpty(zipCodeEntry.Text))
            {
                Weather weather = await WeatherService.GetDataFromService(zipCodeEntry.Text);
                FindViewById<TextView>(Resource.Id.locationText).Text = weather.Title;
                FindViewById<TextView>(Resource.Id.tempText).Text = weather.Temperature;
                FindViewById<TextView>(Resource.Id.windText).Text = weather.Wind;
                FindViewById<TextView>(Resource.Id.visibilityText).Text = weather.Visibility;
                FindViewById<TextView>(Resource.Id.humidityText).Text = weather.Humidity;
                FindViewById<TextView>(Resource.Id.sunriseText).Text = weather.Sunrise;
                FindViewById<TextView>(Resource.Id.sunsetText).Text = weather.Sunset;
            }
        }
    }
}
