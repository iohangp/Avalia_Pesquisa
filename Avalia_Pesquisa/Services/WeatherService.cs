using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Avalia_Pesquisa.Models;

namespace Avalia_Pesquisa.Services
{
    public class WeatherService
    {

        public static async Task<dynamic>GetDataFromService(string zipCode)
        {
            //Sign up for a free API key at http://openweathermap.org/appid  

            //api.openweathermap.org / data / 2.5 / forecast / daily ? lat ={ lat}&lon ={ lon}&cnt ={ cnt}
            //Parâmetros:lat, lon coordenadas da localização do seu interesse, cnt número de dias retornados(de 1 a 16)

            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://api.openweathermap.org/data/2.5/weather?zip="
                + zipCode + ",us&appid=" + "YOUR KEY HERE" + "&units=imperial");

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }


            //if (data["weather"] != null)
            //{
            //   Weather weather = new Weather();
            //   weather.Title = (string)data["name"];
            //   weather.Temperature = (string)data["main"]["temp"] + " F";
            //   weather.Wind = (string)data["wind"]["speed"] + " mph";
            //   weather.Humidity = (string)data["main"]["humidity"] + " %";
            //   weather.Visibility = (string)data["weather"][0]["main"];

            //   DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            //   DateTime sunrise = time.AddSeconds((double)data["sys"]["sunrise"]);
            //   DateTime sunset = time.AddSeconds((double)data["sys"]["sunset"]);
            //   weather.Sunrise = sunrise.ToString() + " UTC";
            //   weather.Sunset = sunset.ToString() + " UTC";
            //   return weather;
            //}
            //else
            //{
            //    return null;
            //}

            return null;
            }

        }
    }
 