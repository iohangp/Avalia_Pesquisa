using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace Avalia_Pesquisa
{
    public class WeatherService
    {

        public static async Task<dynamic> getDataFromService(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }

            return data;
        }

        public static async Task<Weather> GetWeather(string zipCode)
        {

            var locator = CrossGeolocator.Current;

            Position position = null;
            locator.DesiredAccuracy = 100;
            position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);

            //Sign up for a free API key at http://openweathermap.org/appid  
            string key = "c7622b2b44f3a3bfabcb09671b11c96e";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?" +
                "lat="+ position.Latitude + "&lon="+ position.Longitude + 
                "&appid=" + key + "&units=metric";

            dynamic results = await getDataFromService(queryString).ConfigureAwait(false);

            Weather weather = new Weather();
           
            if (results["cod"] == 200)
            {

                weather.Title = (string)results["name"];
                weather.Temperature = (string)results["main"]["temp"] + " °C";
                weather.Wind = (string)results["wind"]["speed"] + " km/h";
                weather.Humidity = (string)results["main"]["humidity"] + " %";
                weather.Visibility = (string)results["weather"][0]["main"];
                weather.Clouds = (string)results["clouds"]["all"] + " %";
                weather.Atualizacao = (string)results["coord"]["dt"];

                DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
                DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
                weather.Sunrise = sunrise.ToString() + " UTC";
                weather.Sunset = sunset.ToString() + " UTC";
                    
                return weather;
            }
            else
            {
                weather.Title = "";
                return weather;
            }           
            
                

        }

    }
}
